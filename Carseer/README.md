Carseer Model API

The Carseer Model API is a C# .NET Core application that provides a REST API to retrieve car models for a specific make and manufacture year. 
This API allows you to obtain a list of car models produced in a particular year for a given car make.

** Getting Started
** Prerequisites
Before you can run the Carseer Model API locally, make sure you have the following prerequisites installed:
 
 -.NET Core SDK
 - CsvHelper
 - Newtonsoft.Json

 The API should now be running locally at https://localhost:7009.

** Usage
API Endpoint
The API endpoint for retrieving car models is:

 - https://localhost:7009/api/models

 ** Request
To retrieve car models, make a GET request to the API endpoint with the following query parameters:

- modelyear: The manufacture year of the car (e.g., 2015).
- make: The car make for which you want to retrieve models (e.g., Lincoln).

Example Request:

GET https://localhost:7009/api/models?modelyear=2015&make=Lincoln

Response
The API will respond with a JSON object containing an array of car models for the specified make and year.

Example Response:
{
  "Models": [
    "MKZ",
    "MKS",
    "MKT",
    "MKT",
    "MKX",
    "Navigator",
    "MKC"
  ]
}

** CSV Data
To obtain the car make ID required for the API, you can refer to the CarMake.csv file included in this repository.
The user of the API should pass the car make as input, and the API will internally use the car make ID to retrieve the models.

** How it Works
This API internally calls the GetModelsForMakeIdYear API, which requires car make ID and manufacture year as parameters. 
It uses the CarMake.csv file to map car makes to their corresponding IDs.