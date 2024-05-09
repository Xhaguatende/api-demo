[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=Xhaguatende_api-demo)](https://sonarcloud.io/summary/new_code?id=Xhaguatende_api-demo)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Xhaguatende_api-demo&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Xhaguatende_api-demo)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Xhaguatende_api-demo&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Xhaguatende_api-demo)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Xhaguatende_api-demo&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Xhaguatende_api-demo)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Xhaguatende_api-demo&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Xhaguatende_api-demo)

# API Demo
This is a simple API demo using .NET and MongoDB.


# Pre-requisites
- MongoDB<br />
**Option 1.** Install MongoDB from [here](https://www.mongodb.com/try/download/community)
**Option 2.** Use Docker.<br />
See the **docker-compose.yml** sample file in the folder: **./docker/infrastructure/**<br />
Just execute **docker-compose up -d** from that location.

- Node.js (for migrations)<br />
Install Node.js from [here](https://nodejs.org/en/download/) (use version **20.x.x** or higher)<br />

# Migrations
The migrations are located in the folder: **./mongodb/migrations/**<br />
It uses the tool [migrate-mongo](https://www.npmjs.com/package/migrate-mongo).<br />
Follow the instructions in the link to install it.<br />
From the referred folder, execute the following command to run the migrations:
```bash
migrate-mongo up
```

This will create the database (_product-catalogue_) and relevant elements (e.g., views).

# Running the API
The API is a .NET Core 8.0 application.<br />
It uses the **appsettings.developement.json** file to configure the connection to the MongoDB database.<br />

Open the solution in Visual Studio and run the project **ApiDemo.Api**.<br />

# Accessing the API
The API is a simple CRUD for products.<br />
It requires users to be authenticated to access the endpoints (except **api/accounts**).<br />

Swagger can be used to access the endpoints: **https://localhost:7225/swagger/index.html**<br />

Use the **api/accounts/register** endpoint to create/register an account. <br />
Example:
```bash
curl -X 'POST' \
  'https://localhost:7225/api/accounts/register' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "username@mail.com",
  "confirmPassword": "Password123!",
  "password": "Password123!"
}'
````

Use the **api/accounts/sign-in** endpoint to authenticate and obtain the access token.<br />
Example:
```bash
curl -X 'POST' \
  'https://localhost:7225/api/accounts/sign-in' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "username@mail.com",
  "password": "Password123!"
}'
````

Use the access token in the **Authorization** header to access the other endpoints.<br />
Example:
```bash
curl -X 'GET' \
  'https://localhost:7225/api/categories' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQ...'
````

# Tests
All test projects are located under the solution folder **tests**.<br />
It requires MongoDB to be running.<br />

Run the tests using Visual Studio or the command line (**dotnet test** from the **./src** folder).<br />
