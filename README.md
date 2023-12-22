# Proof of Concept for PrescriberPoint

The **BallastLane** company is helping **PrescriberPoint** design some features to its platform where employees can create users to manage **Medications**. So, once a user is created and logged in, the employee will be able to execute operations like:
* Create a Medication
* Update a Medication
* Delete a Medication

It is important to highlight some services not require authentication to see Medication info:
* See a Medication list
* Search a Medication by Id


# User Stories

## 1. Create User
As an Employee <br>
I want to have access to the PrescriberPoint system <br>
So I can log in to the platform <br>

**Acceptance Criteria** <br>
Given a form to create my user <br>
When I submit the required info <br>
Then my system's user is created successfully <br>

## 2. Log in
As a system's User <br>
I want to log in to the platform <br>
So I can access to it and receive all my user's info <br>

**Acceptance Criteria** <br>
Given a form to log in to the system <br>
When I access the platform <br>
Then I receive my user's info by including a valid token to perform the Medication operations<br>

## 3. Create a Medication
As a system's User <br>
I want to create a Medication <br>
So I can have more products in the inventory <br>

**Acceptance Criteria** <br>
Given a form to create a Medication <br>
When I submit the required info <br>
Then the new Medication is created successfully <br>

## 4. Update a Medication
As a system's User <br>
I want to update a Medication's info <br>
So I can have the products with the most recent details <br>

**Acceptance Criteria** <br>
Given a form to update a Medication <br>
When I submit the required info <br>
Then the Medication is updated successfully <br>

## 5. Delete a Medication
As a system's User <br>
I want to delete a Medication <br>
So I can have the inventory up to date <br>

**Acceptance Criteria** <br>
Given a form to delete a Medication <br>
When I submit the required info <br>
Then the Medication is removed successfully from the platform <br>

## 6. See a Medication list
As an Employee  <br>
I want to see a Medication list <br>
So I can check all the available medications on the platform <br>

**Acceptance Criteria** <br>
Given a form to request a Medication list <br>
When I ask for the information <br>
Then the Medication list is shown without being logged into the system <br>

## 7. See a Medication
As an Employee  <br>
I want to see a specific Medication <br>
So I can check all the details <br>

**Acceptance Criteria** <br>
Given a form to see a Medication's details <br>
When I ask for the information <br>
Then the Medication's data is shown without being logged into the system <br>


# Project Configuration

1. Make sure you have installed Visual Studio 2022
2. Clone this repo in the machine where you are going to work with
3. In the **Code** folder, open the ```BallastLane.sln```
4. Check the solution builds correctly <br>
4.1 If you are facing any problems at the moment of building the solution, please fix them before continuing with other steps.

## Database Configuration

1. In the repo that you have just cloned, in the **Code** folder you will find the ```docker-compose.yml``` file
2. Under this location, open a ```Console/Terminal``` window and run the ```docker compose up -d``` command <br>
2.1 Way to go! A container is running with an SQL Server instance. Check this out!
3. In the [appsettings.Development.json](https://github.com/LuisFMoraM/BallastLane/blob/main/Code/MedicationApi/appsettings.Development.json) file, you will see the ```ConnectionStrings``` section to check the DB name. <br>
3.1 Right now, with a tool to work with the Database (Ex: SQL Server Management Studio), you can connect with the ```localhost``` instance + credentials <br>
3.2. The ```PrescriberPoint``` DB should be created. If not, run this [initial-db-script](https://github.com/LuisFMoraM/BallastLane/blob/main/Code/DataAccess/CreateDatabase/initial-db-script.sql)

## Run the project

In Visual Studio 2022, you should have opened the ```BallastLane.sln``` (Section: Project Configuration, Steps: 3-4)
1. In the Solution, Set **Startup Projects**. Configure ```Medication.Api & User.Api``` to start at the same time.
2. Run the project by pressing F5
3. You will see the following Swagger web pages:
* [Medication Api](http://localhost:5096/swagger/index.html)
* [User Api](http://localhost:5038/swagger/index.html) <br>
All the project documentation will be there, including the Endpoints, parameters, etc <br>
4. Now, you are ready to consume the services exposed by the APIs <br>
4.1 Follow the **Swagger** Documentation to create the requests<br>
4.2 For those that require **Authorization**, you can use Postman to send the generated Token [(See US #2)](#2-log-in) <br>