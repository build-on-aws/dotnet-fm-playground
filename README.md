# .NET FM Playground

.NET FM Playground is a .NET MAUI Blazor sample application showcasing 
how to leverage Amazon Bedrock from C# code. As any sample application, it is not production-ready.
It is provided for the sole purpose of illustrating of .NET and C# developers can leverage Amazon Bedrock
to build generative AI enabled applications.

Amazon Bedrock is a fully managed 
service that offers a choice of high-performing foundation models (FMs) 
from leading AI companies like AI21 Labs, Anthropic, Cohere, Meta, Stability AI, 
and Amazon via a single API.

It also offers Knowledge Bases and Agents for Amazon Bedrock to speed up the implementation of Retrieval Augmented Generation pattern and execute complex tasks.

In order to let you test and interact with the different foundation models and advanced features, the .NET FM Playground offers you five playgrounds:
* A text playground
* A chat playground
* A voice chat playground
* An image playground
* An agent playground

In addition, it also lists and displays the foundation models you have access to and their characteristics.

![Screenshot of the chat playground](/doc/img/ChatPlayground.png "Screenshot of the chat playground")

![Screenshot of the image playground](/doc/img/ImagePlayground.png "Screenshot of the image playground")

## Dev Environment Prerequisite

This sample has been developed using:
* .NET 8.0
* .NET MAUI 8.0

You need to install .NET 8.0 SDK. 

For .NET MAUI 8.0:
* if you use Visual Studio 2022, install the .NET Multi-platform App UI development
workload from the Visual Studio Installer
* otherwise, you can install maui-windows and maui-maccatalyst workload running the 
following commands
	```
	dotnet workload install maui-windows
	dotnet workload install maui-maccatalyst
	```

This sample has been developed using Visual Studio 2022 17.7.3 on Windows 11. While 
it should work on MacOS and in other IDEs supporting .NET MAUI development, we can't 
guarantee.

## AWS Account prerequisite

### Identity & Access Management

For now, the .NET FM Playground requires you to configure your default AWS profile with 
credentials giving you access to Amazon Bedrock.

To learn more about granting programmatic access and setting up permissions, read the following
documentations:
* https://docs.aws.amazon.com/bedrock/latest/userguide/setting-up.html
* https://docs.aws.amazon.com/bedrock/latest/userguide/security-iam.html

### Model Access

An AWS Account does not have access to models by default. An Admin user with IAM access permissions
can add access to specific models using the model access page.

To learn more about managing model access, read the following documentation:
* https://docs.aws.amazon.com/bedrock/latest/userguide/model-access.html 

### Model Access

To use the agent playground, you first need to create an agent in your AWS account.

To learn more about creating and managing agents for Amazon Bedrock, read the following documentation:
* https://docs.aws.amazon.com/bedrock/latest/userguide/agents.html

## Repository Structure

The repository has the following structure
* **src:** contains the solution
	* **DonetFMPlayground.Core:** contains a library adding method extensions to the 
	AmazonBedrockRuntimeClient object from the AWS SDK for .NET
	* **DotnetFMPlayground.App:** contains the .NET MAUI Blazor application
* **doc:** contains documentation
	* **img:** contains screenshots and images


## Security

See [CONTRIBUTING](CONTRIBUTING.md#security-issue-notifications) for more information.

## License

This library is licensed under the MIT-0 License. See the LICENSE file.

