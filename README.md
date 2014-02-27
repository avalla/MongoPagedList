MongoPagedList
=========

A PagedList extension that works with MongoCursor.

  - MongoCursor<T> Pagination
  - Type Conversion (using AutoMapper)

Version
----

1.0


Usage with Type Conversion
--------------

```sh
var cursor = ;
var pageNumber = 1;
var pageSize = 25;
Collection.Find(Query.EQ("Field", "Value"));
var model = new MongoPagedList<TSource, TDestination>(query, pageNumber, pageSize);
```
Usage without Type Conversion
--------------

```sh
var cursor = ;
var pageNumber = 1;
var pageSize = 25;
Collection.Find(Query.EQ("Field", "Value"));
var model = new MongoPagedList<T>(query, pageNumber, pageSize);
```

License
----

MIT


Author
----
Andrea Valla https://github.com/avalla

Thanks
----
PagedList https://github.com/troygoode/PagedList

AutoMapper https://github.com/AutoMapper/AutoMapper

    