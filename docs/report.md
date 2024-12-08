## Design and architecture

### Domain model

Provide an illustration of your domain model.
Make sure that it is correct and complete.
In case you are using ASP.NET Identity, make sure to illustrate that accordingly.

### Architecture — In the small

Illustrate the organization of your code base.
That is, illustrate which layers exist in your (onion) architecture.
Make sure to illustrate which part of your code is residing in which layer.

### Architecture of deployed application

Illustrate the architecture of your deployed application.
Remember, you developed a client-server application.
Illustrate the server component and to where it is deployed, illustrate a client component, and show how these communicate with each other.

### User activities

Illustrate typical scenarios of a user journey through your _Chirp!_ application.
That is, start illustrating the first page that is presented to a non-authorized user, illustrate what a non-authorized user can do with your _Chirp!_ application, and finally illustrate what a user can do after authentication.

Make sure that the illustrations are in line with the actual behavior of your application.

### Sequence of functionality/calls trough _Chirp!_

With a UML sequence diagram, illustrate the flow of messages and data through your _Chirp!_ application.
Start with an HTTP request that is send by an unauthorized user to the root endpoint of your application and end with the completely rendered web-page that is returned to the user.

Make sure that your illustration is complete.
That is, likely for many of you there will be different kinds of "calls" and responses.
Some HTTP calls and responses, some calls and responses in C# and likely some more.
(Note the previous sentence is vague on purpose. I want that you create a complete illustration.)

## Process

### Build, test, release, and deployment

Illustrate with a UML activity diagram how your _Chirp!_ applications are build, tested, released, and deployed.
That is, illustrate the flow of activities in your respective GitHub Actions workflows.

Describe the illustration briefly, i.e., how your application is built, tested, released, and deployed.

### Team work

Show a screenshot of your project board right before hand-in.
Briefly describe which tasks are still unresolved, i.e., which features are missing from your applications or which functionality is incomplete.

Briefly describe and illustrate the flow of activities that happen from the new creation of an issue (task description), over development, etc. until a feature is finally merged into the `main` branch of your repository.

### How to make _Chirp!_ work locally

In order to clone the repository you have to run the following command, which requires you to have git installed:

```git clone https://github.com/ITU-BDSA2024-GROUP1/Chirp.git```

Once you cloned the repository you will have to setup some user-secrets since they are used for third party login. Start by locating the files on your pc and once you are in the Chirp folder you should be able to run the following commands after having dotnet-ef installed (If you have installed dotnet-ef you can skip the first command):

```dotnet tool install --global dotnet-ef```

```dotnet user-secrets set "auth_github_clientId" "Ov23liRWZA8rujaSnUGT" --project "src/Chirp.Razor/Chirp.Razor.csproj"```

```dotnet user-secrets set "auth_github_clientSecret" "6f43c9d347116d35557b9a98133177b520f97178" --project "src/Chirp.Razor/Chirp.Razor.csproj"```

To check if the user-secrets are set you can run the following command:

```dotnet user-secrets list --project "src/Chirp.Razor/Chirp.Razor.csproj"```

In case the user-secrets haven't been set you will have to set them manually which you do the following way:

1. Head to the following location and ensure a folder ```UserSecrets``` exists: C:\Users\\\<user>\AppData\Roaming\Microsoft\, where user is your pc user.
2. Then you will have to create a folder within ```UserSecrets``` that is called: 7fac5a3e-b457-40f0-8c72-57166e5bc39f 
3. Inside that folder you will have to create a file: secrets.json that will have to contain:
```json
{
  "auth_github_clientId": "Ov23liRWZA8rujaSnUGT",
  "auth_github_clientSecret": "6f43c9d347116d35557b9a98133177b520f97178"
}
```
4. Now you can go back to the Chirp folder and try the command that lists all of the user-secrets for the applications as with the other method.

### How to run test suite locally

List all necessary steps that Adrian or Helge have to perform to execute your test suites.
Here, you can assume that we already cloned your repository in the step above.

Briefly describe what kinds of tests you have in your test suites and what they are testing.

## Ethics

### License

State which software license you chose for your application.

### LLMs, ChatGPT, CoPilot, and others

State which LLM(s) were used during development of your project.
In case you were not using any, just state so.
In case you were using an LLM to support your development, briefly describe when and how it was applied.
Reflect in writing to which degree the responses of the LLM were helpful.
Discuss briefly if application of LLMs sped up your development or if the contrary was the case.