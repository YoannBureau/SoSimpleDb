# SoSimpleDb
A simple way to store data in your applications. If you want to store collections and you don't want to deal with SQL or avoid learning NoSQL, SoSimpleDb is made for you.

# Build Status: [![Build status](https://ci.appveyor.com/api/projects/status/2nlklg6vo9094kwb?svg=true)](https://ci.appveyor.com/project/YoannBureau/sosimpledb)
### Branches
|Branch|Status|
|---|---|
|master|[![Build status](https://ci.appveyor.com/api/projects/status/2nlklg6vo9094kwb/branch/master?svg=true)](https://ci.appveyor.com/project/YoannBureau/sosimpledb/branch/master)|
|develop|[![Build status](https://ci.appveyor.com/api/projects/status/2nlklg6vo9094kwb/branch/develop?svg=true)](https://ci.appveyor.com/project/YoannBureau/sosimpledb/branch/develop)|

# Install
```Install-Package SoSimpleDb```

# How to use
Assuming the following class:
```c#
public class Person : SoSimpleDbModelBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```
**Note : Your model classes must inherit from the *SoSimpleDbModelBase* abstract class.** That simply adds an Id property on your class definition.

## Insert
```c#
var person = new Person() { Id = 1, FirstName = "Peter", LastName = "Smith" };
SoSimpleDb<Person>.Instance.Insert(person);
```

## Insert many
```c#
var persons = new List<Person>();
//Fill your collection with instances of you model class
SoSimpleDb<Person>.Instance.Insert(persons);
```

## Update
```c#
SoSimpleDb<Person>.Instance.Update(myInstanceOfPerson);
```

## Delete
```c#
SoSimpleDb<Person>.Instance.Delete(myInstanceOfPerson.Id);
```

## SelectAll
```c#
var persons = SoSimpleDb<Person>.Instance.SelectAll();
```

## Select by Id
```c#
var person = SoSimpleDb<Person>.Instance.Select(1);
```

## Select by Func
```c#
Func<Person, bool> searchFunc = (x) => x.LastName.Contains("Smith");
var persons = SoSimpleDb<Person>.Instance.Select(searchFunc);
```

## Count
```c#
var personCount = SoSimpleDb<Person>.Instance.Count();
```

# Where the hell my data is stored?
Calm down. It's stored in a JSON file named `Data.ssdb` located by default in the path of your executing assembly.

## Add a custom path for the storage file
Simply add a new AppSetting in your application configuration file with the `SoSimpleDb.CustomFile` key. For example:
```xml
<add key="SoSimpleDb.CustomStorageFile" value="C:\Users\MyUser\Documents\test.ssdb"/>
```
