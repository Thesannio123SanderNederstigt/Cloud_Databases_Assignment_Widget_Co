# Cloud_Databases_Assignment_Widget_Co
Cloud Databases Assignment Git Repository

This application is currently split into two API azure function applicaties (one for all get and one for all post requests)

The way the third processing azure function app is set up (currently just for the posting of orders) is technically how all requests to the sql database should be done (so pretend like all requests are posted to a queue and then read and processed into a SQL db).
This function reads a queue DTO objects are posted to and then processes these into an SQL database for orders to relieve these.

