**C# Bronze Badge**
Todo API + API Tests using NUnit Restsharp
Simple TodoAPI created following the MSLearn [article](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio). Added an API test framework for the REST service endpoints using NUnit RestSharp plus FluentAssertions.

**Nuget packages:**
* RestSharp 110.2.0
* NUnit 3.13.3
* Newtonsoft.Json 13.0.3
* FluentAssertions 6.12.0
* NUnit.ConsoleRunner 3.17.0

**Instructions:** 
1. Clone repository
2. Install nuget packages
3. Configure NUnit Console Runner
    * Get the NUnit Console Runner from this [link](https://github.com/nunit/nunit-console/releases). Make sure to use the `NUnit.Console-3.17.0.msi`
    * You can specify the PATH to NUnit Console Runner in your machine: 

            1. Right-click on Windows icon and choose System.
      
            2. Select Advanced system settings.
      
            3. Select `Path` under User variables for User and click Edit.
      
            4. Click New and add the path to the tools folder of NUnit Console Runner i.e.
              C:\Users\CarloCandoy\.nuget\packages\nunit.consolerunner\3.17.0\tools
      
            5. Then click OK
      
4. Locate the folder where the Tests.dll is. i.e.
   ```C:\Users\CarloCandoy\Desktop\c-sharp-bronze\TodoApi\Tests\bin\Debug\net8.0\Tests.dll```
5. Run the tests by doing 
```nunit3-console.exe Path\To\Tests.dll```

    e.g. ```nunit3-console.exe C:\Users\CarloCandoy\Desktop\c-sharp-bronze\TodoApi\Tests\bin\Debug\net8.0\Tests.dll```
    
    Or if you did not set the PATH, you can do: 
    `Path\to\nunit3-console.exe Path\to\Tests.dll`
    
    example:
        ``` C:\Users\CarloCandoy\.nuget\packages\nunit.consolerunner\3.17.0\tools\nunit3-console.exe C:\Users\CarloCandoy\Desktop\c-sharp-bronze\TodoApi\Tests\bin\Debug\net8.0\Tests.dll```


**CLI Options**

| Description | Command | Example |
| -------- | -------- | -------- |
| Specify the specific TestFixture to run     |`--test=FILENAME`| `nunit3-console.exe C:\Users\CarloCandoy\Desktop\c-sharp-bronze\TodoApi\Tests\bin\Debug\net8.0\Tests.dll --test=Tests.TestFixtures.EditTodoItemTests`     |
|Run all the Tests|`nunit3-console.exe Path\to\Tests.dll`|`nunit3-console.exe C:\Users\CarloCandoy\Desktop\c-sharp-bronze\TodoApi\Tests\bin\Debug\net8.0\Tests.dll`|

