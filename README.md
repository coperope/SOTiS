# Intoduction
This repository hosts an application that offers a platform for testing student knowledge.
The testing process is based on the knowledge space theory, thus providing teaching staff with tools for creating and managing knowledge spaces.
Online testing procedures are then build on top of the matching knowledge space. 
On the basis of previously made KST-based tests, students' knowledge can be tested in a guided fashion. This sort of guided testing provides various benefits both for students and teachers, which include the insight into the realistic knowledge state of students as a group or an individual.

# Technology, frameworks & algorithms

The system is built as a web application, therefore it's main parts are the back-end service that handles all the business logic and the front-end service which gives access to the system to end users via UI. To handle creation of realistic knowledge spaces, the backend service uses an auxiliary system which processes the resolved students tests using algorithms provided by [an external library](https://github.com/milansegedinac/kst). 

These three parts of the system are built using the following technologies:
- .NET v5 - used to built the main service that handles systems business logic
- FastAPI framework for Python - used to build the auxiliary system for generating knowledge spaces via KST algorithms
- React framework - used to build UI for end-user interaction

Main service is written with clean architecture in mind using command and query pattern.
Depending on the nature of users request, a query or command is created which queries and returns data to user or make changes to it.
Doing this allows us to have separate logic in code making it easier to build on and maintain the app.

# Main features and workflow

Main system participants are the teaching staff - represented by a professor role and students.

The system provides professors with the following activities:
- Registration and login
- Creation of assumed knowledge space. The knowledge space is represented as a directed acyclic graph (DAG) where nodes represent problems in a corresponding domain and directed connections represent dependencies between problems. Each problem in the assumed knowledge space represents a part of the domain that can be mastered as a whole. 
The directed dependency relationship between problems means that the problem on the end part of relation can not be overcome before mastering the problem at the beggining of the relation. If someone already has enough knowledge about a futher problem in a graph it can be assumed he has enough knowledge to solve all the previous problems.
- Creation of exams with multiple questions and multiple answers. One question can have one or more correct answers. Each test is assigned with corresponding assumed knowledge space and each question in a test is assigned with a single problem from the domain.
- Creation of a real knowledge space based on the students answers on the tests. For this purpose the system uses the IITA algorythm. Whereas in a complete system used in the real world scenario this would be done using actual students' answers, in this particular system that behaviour is simulated using a set of answers from PISA tests.
- Comparing assumed and real knowledge spaces with visual and quantitative representation of their diference.
- Visualisation of knowledge state for a particular domain.

Students in the system have the ability to:
- Register and login to the system
- Take tests and see the results
- Have an insight into his or hers current knowledge state, based on the provided answers in a certain test. 
