//
// SpecificationExpressionTransformationIntegrationTests.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2020 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Dialect;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;
using CSF.Specifications.Tests.Stubs;
using CSF.Specifications;
using NHibernate.Linq;
using System.Linq;

namespace CSF.Specifications.Tests.Specifications
{
    [TestFixture,NonParallelizable, Description("Integration tests which use NHibernate to use specifications with an actual database, to verify that specification expressions are compatible.")]
    public class SpecificationExpressionTransformationIntegrationTests
    {
        Configuration config;
        ISessionFactory sessionFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            config = new Configuration();

            config.DataBaseIntegration(db => {
                db.Dialect<SQLiteDialect>();
                db.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
            });
            config.AddMapping(GetMapping());

            sessionFactory = config.BuildSessionFactory();
        }

        HbmMapping GetMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            sessionFactory?.Dispose();
        }

        [Test]
        public void Transform_expression_returns_a_spec_which_is_compatible_with_NHibernate()
        {
            using (var session = sessionFactory.OpenSession())
            {
                SetupQueryIntegrationTest(session);

                var personSpec = Spec.Expr<Person>(x => x.Name == "Jane Doe");
                var petSpec = personSpec.Transform(t => t.To<Pet>(x => x.Owner));

                Assert.That(session.Query<Pet>().Where(petSpec).Select(x => x.Identity).FirstOrDefault(), Is.EqualTo(1));
            }
        }

        void SetupQueryIntegrationTest(ISession session)
        {
            using (var tran = session.BeginTransaction())
            {
                var exporter = new SchemaExport(config);
                exporter.Execute(false, true, false, session.Connection, null);
                tran.Commit();
            }

            using (var tran = session.BeginTransaction())
            {
                var pet = new Pet
                {
                    Identity = 1,
                    Owner = new Person { Identity = 1, Name = "Jane Doe" }
                };
                session.Save(pet);
                session.Save(pet.Owner);
                session.Flush();

                tran.Commit();
            }

            session.Clear();
        }
    }
}
