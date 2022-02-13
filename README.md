This repository contains the solution for the code assignment

A note: this requirement hasn't been implemented. The solution doesn't contain any language checks
>The crawler is only meant to work on the english language.

To run the solution, you need to execute `docker-compose up --build --force-recreate` command at the root of the repository. Please check `docker-compose.yml` file for all bootstrap details. Please note that MongoDB has been chosen as data storage.

The solution contains following projects:
- `CodeAssignment.Orchestrator` - API project that calls different crawler agents and stores results in the database.
- `CodeAssignment.Crawler.Abstractions` - common interface for crawler logic.
- `CodeAssignment.Crawler.Wikipedia` - crawler agent capable for processing Wikipedia URLs only.
- `CodeAssignment.Crawler.Universal` - crawler agent capable for processing any URLs.
- `CodeAssignment.Crawler.Wikipedia.RestClient` - a package that contains client for Wikipedia crawler agent API.
- `CodeAssignment.Crawler.Universal.RestClient` - a package that contains client for Universal crawler agent API.

After bootstrap, you will be able to test the system at http://localhost:5001/swagger/index.html. This Swagger UI provides 2 endpoints (for submitting URLs to crawl and read the latest results respectively)

Time spent on this solution is approximately 3 hours.