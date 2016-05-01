A C# implementation of an interval tree datatype based off the description in *Introduction to Algorithms* (Cormen, T. H., Leiserson, C. E., Rivest, R. L. & Stein, C).

How this implementation might differ from others is that is implemented as a generic type (specifically `IIntervalTree<T>`).
This allows you to get interval tree behavior on your already-existing business objects that may already represent intervals of some type.

API documentation can be found [here](http://tinister.github.io/IntervalTree/api/index.html).
