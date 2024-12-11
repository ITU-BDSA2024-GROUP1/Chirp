## Design and architecture

### Domain model

![](images/DomainModel.png "Domain model")

As illustrated, our domain model consists of three key entities: **Author**, **Cheep**, and **Follow**. The model integrates seamlessly with ASP.NET Identity for authentication and authorization, as demonstrated by the inheritance of the ``Author`` entity from ``IdentityUser``. This ensures access to built-in Identity properties such as ``UserName``, ``Email``, and password management features.

The **Author** entity is central to the model and includes a collection of associated cheeps, representing the one-to-many relationship between an author and their posts.

The **Cheep** entity encapsulates information about the author and the cheep itself, including:
- **CheepId**: A unique identifier for each cheep.
- **Text**: The message content, constrained to 160 characters for brevity and readability.
- **TimeStamp**: The creation time of the cheep.
- **AuthorId**: A foreign key linking the cheep to its author.

The **Follow** entity represents a many-to-many relationship between authors, where one author can follow another. This entity includes:
- **FollowerId**: The ID of the author who initiates the follow action.
- **FollowedId**: The ID of the author being followed.
- **Relationships** to the corresponding ``Author`` entities (``Follower`` and ``Followed``).

The **Follow** domain is primarily designed for write operations rather than read operations. This approach is suitable given our context, where the number of users is minimal, and complex read optimizations are not required.

#### Validation and Constraints
To maintain data integrity:
- **Text** in the **Cheep** entity is validated to ensure it does not exceed 160 characters.
- Composite primary keys (``FollowerId`` and ``FollowedId``) in Follow prevent duplicate entries for the same follow relationship.

### Architecture — In the small

![](images/Architecture.png "Figure 1: Architecture")

As illustrated, our code base is organized following an onion architecture, emphasizing separation of concerns and a dependency-inversion approach. The architecture consists of four distinct layers:
1. **Domain Layer** \
This is the core of our application, containing the domain entities, core business rules, and logic. This layer is framework-agnostic, ensuring its independence from external dependencies.

2. **Repository Layer** \
This layer handles persistence and data access. It includes interfaces and their implementations for managing the storage and retrieval of domain entities. Examples include Entity Framework Core repositories for database operations.

3. **Service Layer** \
The Service Layer acts as the mediator between the Repository Layer and the Application Layer. It contains the application’s business logic, orchestrates calls to repositories, and prepares data for the Application Layer.

4. **Application and Tests Layer** \
This outermost layer contains the presentation logic, implemented as Razor Pages or Views, which interact with end-users. It also includes test projects to verify the functionality and integrity of the system, covering unit, integration, and end-to-end tests.

Each layer strictly depends on the layer beneath it, ensuring adherence to onion architecture principles. For example: 
- The Service Layer depends on the Repository Layer for data operations but not vice versa.
- The Repository Layer depends on abstractions defined in the Domain Layer, not concrete implementations.

This structure ensures flexibility, testability, and maintainability of the code base.

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

We have chosen to use an MIT license for our project since it is an open source project that is open to the public on GitHub, where anybody is allowed to use it if they choose to as long as they credit us.

### LLMs, ChatGPT, CoPilot, and others

State which LLM(s) were used during development of your project.
In case you were not using any, just state so.
In case you were using an LLM to support your development, briefly describe when and how it was applied.
Reflect in writing to which degree the responses of the LLM were helpful.
Discuss briefly if application of LLMs sped up your development or if the contrary was the case.