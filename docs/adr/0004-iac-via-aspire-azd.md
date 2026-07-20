# Infrastructure as code via Aspire + azd from day one

Azure resources (the Function App, its storage account, Application Insights, and any per-app resources) are provisioned via Bicep generated and maintained through the Aspire AppHost model, and deployed with the Azure Developer CLI (`azd`) — rather than creating the resources manually once, outside the repo.

This is more setup than strictly necessary for what is currently a single, long-lived resource. It was chosen deliberately so the infrastructure can be expanded or recreated easily as new apps, and their supporting Azure resources, are added over time.
