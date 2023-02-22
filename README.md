# Cloud_Databases_Assignment_Widget_Co
This is a respository for my take on a school assignment for Cloud Databases ()

This assignment reads as follows:
```
Scenario:

Widget and Co product widget - have decided to transfer their website from a local ISP hosting company to the cloud. The site has become very popular during the past few years and can experience heavy loads at peak hours. It is suggested that the move to Azure could alleviate some of the occurring problems.
During peak hours a huge amount of traffic is generated by online users who place their orders. In the past the order placing process has been the main culprit in the sluggish performance of the website.
The orderdate is stored; after an order is shipped the shipping date is again stored. these are used to calculate orderprocessed metric. 
As Widget and Co is part of the conglomerate Wiley and Co the user info is shared with another department.
The product specification is kept in a traditional SQL database, Images of the product are also stored. 
The site also runs a forum part where users can post reviews of the products (anonymously). At a later stage the marketing department would use the reviews to profile a new range of products. 

Your task:

The Online web store will be hosted (and run) in a cloud environment (Azure). Design a proof of concept (in C# Web API with at least 2 Azure Functions) of the application where special attention is paid to the design of the Cloud Database architecture. 
The proof of concept does not require a frontend!
```

I created three azure function applications for this assignment.

The application is split into two API azure function applications (one for all get and one for all post requests) and another function to process requests using Azure storage account queues.

The way this third processing azure function app is set up (currently just for the posting of orders) is technically how I wanted to handle all requests to the sql database (that way all requests would be posted to a queue and then read and processed into a SQL db).
This function reads a queue where DTO objects are send/posted to and then processes these into an SQL database for orders to relieve these other api function endpoints.

The swagger specifications for these api azure function applications can be found here:

Post API: https://cdb577208post.azurewebsites.net/api/swagger/ui

Read API: https://cdb577208read.azurewebsites.net/api/swagger/ui

The processing function app can be found here: 
https://cdb577208processing.azurewebsites.net/


