# Product Manager v1.1

Requirement

As a user, I want to add a product category 4
As a user, I want to list product categories 6
As a user, I want to add a product to a product category 7
As a user, I want to organize product categories hierarchically


Restrictions
The following restrictions must be followed when building the application:

SQL Server must be used.
ADO.NET must be used to retrieve, store, update and delete data from the database.
There must be reference integrity in the relationship between a product and a category.
When deleting a category, all relationships between it and the included products must be deleted.
When deleting a product, all incoming relationships between it and all the categories it is included in must be deleted.
