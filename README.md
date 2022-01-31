# Microservices with MassTransit Syllabus

Create a microservice-architected restaurant application.

We will have an inventory API with one service per ingredient.  Each ingredient will support a method of both modifying ([consumer](https://masstransit-project.com/usage/consumers.html)) and retrieving ([producer](https://masstransit-project.com/usage/producers.html)) inventory.

[Creating](https://masstransit-project.com/usage/sagas/) & “[cooking](<https://masstransit-project.com/usage/sagas/>)” an order will also be supported, with each ingredient-service also responsible for preparing the ingredient.

1. Advanced Preparation
    1. Clone Repository
    2. Create/Push Own Branch
    3. Confirm Able to Start Solution Under Docker-Compose profile.
2. Docker, Docker Compose, and Local Development
3. Introduction to Consumers
    1. Concept
    2. Add the Lettuce service to the solution
    3. API Registration of the Lettuce Service
    4. Respecting the Timeout
    5. Homework: Add a Tomato service to the solution
4. Introduction to Producers
    1. Concept
    2. Add the Lettuce Producer to the Lettuce service
    3. Homework:
        1. Add a Tomato service to the solution
        2. Aggregate the Tomato & Lettuce in an API call
5. Introduction to Sagas (Distributed Transactions)
    1. Concepts
        1. Instance
        2. States
        3. Events
    2. Activities
        1. Discuss
        2. Add Activities
    3. Activity Compensation
        1. Discuss
        2. Demonstrate Compensation
        3. Providing Saga Status
6. Testing
7. Export Service 3000 Overview