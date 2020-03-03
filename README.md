# The specification pattern
This library presents an implementation of **[the specification pattern]**, inspired loosely by
[the work by Vladimir Khorikov] and extended for compatibility with [the NHibernate ORM]
based upon [this article by Pete Montgomery].

## Why use specifications?
At its simplest, **a specification** encapsulates a **predicate** for an object type, where that
predicate represents a piece of reusable business logic.  For example:

```csharp
// 'deliveries' is an IQueryable<Delivery>, perhaps from an ORM
// 'dateToday' is a DateTime, perhaps DateTime.Today

var lateDeliveries = deliveries
    .Where(x => !x.Cancelled
                && !x.Received
                && x.DispatchDate + TimeSpan.FromDays(30) < dateToday)
    .ToList();
```

This is some fairly standard Linq to filter a query of deliveries just for those which aren't
cancelled, aren't received and where more than 30 days has elapsed since they were dispatched.

What if that definition of "a late delivery" is a commonly-used concept?  What if *many classes*
need to query by late deliveries, but also need to add other criteria?  We certainly don't want
to duplicate that logic across them all.  What we need is a way to put that predicate (the
logic inside the `Where`) into a reusable class, which allows us to combine it with other
similar logic.

## Using a specification class
A specification class, created using this package looks like this:

```csharp
using CSF.Specifications;
using System.Linq.Expressions;

public class DeliveryIsLate : SpecificationExpression<Delivery>
{
    public override Expression<Func<Delivery, bool>> GetExpression()
    {
        return x => !x.Cancelled
                    && !x.Received
                    && x.DispatchDate + TimeSpan.FromDays(30) < dateToday;
    }
}
```

Our example above immediately becomes more clear (it will now require `using CSF.Specifications;`).

```csharp
var lateDeliveries = deliveries
    .Where(new DeliveryIsLate())
    .ToList();
```

## Specifications may use constructor parameters
The specification class shown above has no constructor parameters.  Say that the number of
days since dispatch is variable and needs to be a parameter, it's not always thirty. It is
simple to move the hard-coded 30 to a `private readonly` field and to initialise it from
the class constructor.  This is how to create *parameterized specifications*.

## Specifications may be composed
Let's say that we have another specification for our fictitious `Delivery` class. I'm going to
omit its actual logic but let's imagine it is named `DeliveryIncludesHighValueGoods`.  Now we
want to construct a query for all deliveries which include high-value goods **and** which
are late.

### Combine dynamically
It's possible to combine specifications dynamically, creating a new specification on-the-fly
for one-off usages.

```csharp
using CSF.Specifications;

var lateHighValueDeliveries = deliveries
    .Where(new DeliveryIsLate().And(new DeliveryIncludesHighValueGoods()))
    .ToList();
```

### As a new specification
If this new composite specification is one which needs to be reusable, it's also simple to
create a new specification class which combines them.  This new specification class is reusable
in exactly the same way as any other.

```csharp
using CSF.Specifications;
using System.Linq.Expressions;

public class DeliveryIsLateAndIncludesHighValueGoods : SpecificationExpression<Delivery>
{
    public override Expression<Func<Delivery, bool>> GetExpression()
    {
        return new DeliveryIsLate()
            .And(new DeliveryIncludesHighValueGoods())
            .GetExpression();
    }
}
```

## Full documentation
Full documentation for the specification API can be found on [the project wiki].

## Open source license
All source files within this project are released as open source software,
under the terms of [the MIT license].

[the specification pattern]: https://en.wikipedia.org/wiki/Specification_pattern
[the work by Vladimir Khorikov]: https://enterprisecraftsmanship.com/posts/specification-pattern-c-implementation/
[the NHibernate ORM]: https://nhibernate.info/
[this article by Pete Montgomery]: https://petemontgomery.wordpress.com/2011/02/10/a-universal-predicatebuilder/
[the project wiki]: https://github.com/csf-dev/CSF.Specifications/wiki
[the MIT license]: http://opensource.org/licenses/MIT
