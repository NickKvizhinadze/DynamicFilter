# Dynamic Filter

A package for filtering queries or arrays.

### Installation

Install with Nuget

```
Coming Soon
```

- [Dynamic Filter ](#dynamic-filter)
  - [Installation](#installation)
  - [Getting started](#getting-started)
    - [Filter Methods](#filter-methods)
    - [Create filter model](#create-filter-model)
    - [Use filter](#use-filter)

### Getting started

#### Filter Methods

FilterMethods enum values

| Value               | Description                                                    |
| ------------------- | -------------------------------------------------------------- |
| Equal               | Method for checking equality                                   |
| Contains            | Method for checking if list contains element                   |
| StringContains      | Method for checking if string contains substring               |
| HasValueAndContains | Method for checking if string has value and contains substring |
| HasValueAndEqual    | Method for checking if has value and equal                     |
| GreaterThan         | Method for checking if value is greater then                   |
| GreaterThanOrEqual  | Method for checking if value is equal or greater then          |
| LessThan            | Method for checking if value is less then                      |
| LessThanOrEqual     | Method for checking if value is equal or less then             |
| IsNotNull           | Method for checking if value is not null                       |
| IsNull              | Method for checking if value not null                          |

#### Create filter model

first, you need to specify for what type of array you are trying to filter

```csharp
    [FilterFor(typeof(T))] //Where T is the type/class of array element
```

in the filter model, every property should have FilterMethod attribute, which specifies details for filtering array

```csharp
    [FilterMethod(FilterMethods.Contains, propertyName)]
```

sample of filter model

```csharp
    [FilterFor(typeof(Product))]
    public class ProductFilterModel : BaseFilter
    {
        [FilterMethod(FilterMethods.Contains, nameof(Product.Caption))]
        public List<string> Captions { get; set; }

        [FilterMethod(FilterMethods.Equal)]
        public decimal? Price { get; set; }

        [FilterMethod(FilterMethods.GreaterThanOrEqual, nameof(Product.ReceiveDate))]
        public DateTime? ReceiveDateFrom { get; set; }
    }
```

#### Use filter

filtering data

```csharp
    IQueryable<Product> result = FilterHelper.Filter(productFilter, ProductsList.AsQueryable());
```
