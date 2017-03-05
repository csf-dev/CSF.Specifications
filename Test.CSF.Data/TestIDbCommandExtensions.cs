//
// TestIDbCommandExtensions.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
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
using NUnit.Framework;
using System.Data;
using CSF.Data;
using Moq;
using System.Collections;
using System.Collections.Generic;
using Ploeh.AutoFixture;
using System.Linq;

namespace Test.CSF.Data
{
  [TestFixture]
  public class TestIDbCommandExtensions
  {
    #region fields

    private IFixture _autofixture;

    private Mock<IDbCommand> _command;
    private Mock<IDbDataParameter> _param;
    private Mock<IDataParameterCollection> _params;

    private IList<IDbDataParameter> _addedParameters;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();

      _addedParameters = new List<IDbDataParameter>();

      _command = new Mock<IDbCommand>();
      _param = new Mock<IDbDataParameter>();
      _params = new Mock<IDataParameterCollection>();

      _params.As<IList>()
        .Setup(x => x.Add(It.IsAny<IDbDataParameter>()))
        .Callback((object param) => _addedParameters.Add((IDbDataParameter) param))
        .Returns(1);

      _command.Setup(x => x.CreateParameter()).Returns(_param.Object);
      _command.SetupGet(x => x.Parameters).Returns(_params.Object);

      _param.SetupProperty(x => x.ParameterName);
      _param.SetupProperty(x => x.Value);
    }

    #endregion

    #region tests

    [Test]
    public void AddParameter_generates_parameter_from_command()
    {
      // Arrange
      var com = _command.Object;

      // Act
      com.AddParameter(_autofixture.Create<string>(), _autofixture.Create<string>());

      // Assert
      _command.Verify(x => x.CreateParameter(), Times.Once());
    }

    [Test]
    public void AddParameter_adds_parameter_to_paramaters_collection()
    {
      // Arrange
      var com = _command.Object;

      // Act
      com.AddParameter(_autofixture.Create<string>(), _autofixture.Create<string>());

      // Assert
      _params.Verify(x => x.Add(It.IsAny<IDbDataParameter>()), Times.Once());
    }

    [Test]
    public void AddParameter_add_parameter_with_correct_values()
    {
      // Arrange
      var com = _command.Object;
      var name = _autofixture.Create<string>();
      var value = _autofixture.Create<object>();

      // Act
      com.AddParameter(name, value);

      // Assert
      var added = _addedParameters.Single();
      Assert.AreEqual(name, added.ParameterName, "ParameterName");
      Assert.AreEqual(value, added.Value, "Value");
    }

    #endregion

  }
}

