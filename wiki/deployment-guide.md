- Deployment Guide
    - [Prerequisites](#prerequisites) 
    - [Steps](#Deployment-Steps)
        - [Register Azure AD application](#1-register-azure-ad-application)
        - [Deploy to Azure subscription](#2-deploy-to-your-azure-subscription)
        - [Set-up Authentication](#3-set-up-authentication)
        - [Add Permissions to your app](#4-add-permissions-to-your-app)
        - [Create the Teams app packages](#5-create-the-teams-app-packages)
        - [Install the apps in Microsoft Teams](#6-install-the-apps-in-microsoft-teams)
- - -

# Prerequisites

## Azure Subscription
To begin, you will need: 
* An Azure subscription where you can create the following kinds of resources:  
    * App Service
    * App Service Plan
    * Cosmos Db
    * Bot Service


# Deployment Steps

## 1. Register Azure AD application

Register three Azure AD application in your tenant's directory: one for the Kudos Dashboard, one for the notification bot app and another for the Microsoft Graph.

Log in to the Azure Portal for your subscription, and go to the App registrations blade.

### Approvals Hub Tab

1. Click **New registration** to create an Azure AD application.

* **Name:** Name of your Teams App - if you are following the template for a default deployment, we recommend "Approvals Hub".

* **Supported account types:** Select "Accounts in any organizational directory (Any Azure AD directory - Multitenant)" (refer image below).

* Leave the "Redirect URI" field blank for now.

![](Images/register%20an%20app.png)

2. Click **Register** to complete the registration.

3. When the app is registered, you'll be taken to the app's "Overview" page. Copy the **Application (client) ID**; we will need it later. Verify that the "Supported account types" is set to Multiple organizations.

![](Images/app%20client%20id.png)

5. On the side rail in the Manage section, navigate to the **"Certificates & secrets"** section. In the Client secrets section, click on **"+ New client secret"**. Add a description for the secret, and choose when the secret will expire. Click **"Add"**.

![](Images/add%20a%20client%20secret.png)

6. Once the client secret is created, copy its Value; we will need it later.

![](Images/secret%20value.png)

### Approvals Hub Agent

7. Go back to **"App registrations"**, then repeat steps **2-5** to create another Azure AD application for the notification bot.

* **Name:** Name of your Teams App - if you are following the template for a default deployment, we recommend "Approvals Hub Agent".

* **Supported account types:** Select "Accounts in any organizational directory".

* Leave the "Redirect URI" field blank for now.


### Approvals Hub M365

1. Go back to **"App registrations"**, then repeat steps **1-5** to create another Azure AD application for the notification bot.

* **Name:** Name of your Teams App - if you are following the template for a default deployment, we recommend "Approvals Hub M365".

* **Supported account types:** Select "Accounts in any organizational directory".

* Leave the "Redirect URI" field blank for now.


At this point you should have the following 6 values:

i. Application (client) ID for the Approvals Hub app.
ii. Client secret for the approvals app.
iii. Application (client) Id for the Approvals Hub agent.
iv. Client secret for the Approvals Hub agent.
v. Application (client) Id for the Approvals Hub M365.
vi. Client secret for the Approvals Hub M365.

We recommend that you copy the values, we will need them later.

## 2. Deploy to your Azure subscription

1. Click on the **Deploy to Azure** button below.
   
   [![Deploy to Azure](Images/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fluishdemetrio%2FMyApprovalsHub%2Fmaster%2FDeployment%2Fazuredeploy.json)

2. When prompted, log in to your Azure subscription.

3. Azure will create a "Custom deployment" based on the Approvals Hub ARM template and ask you to fill in the template parameters.

    > **Note:** Please ensure that you don't use underscore (_) or space in any of the field values otherwise the deployment may fail.

4. Select a subscription and a resource group.
   * We recommend creating a new resource group.
   
5. Enter a **Base Resource Name**, which the template uses to generate names for the other resources.
   * The `[Base Resource Name]` must be available. For example, if you select `approvalshub` as the base name, the name `approvalshub` must be available (not taken); otherwise, the deployment will fail with a Conflict error.
   * Please make sure to limit the base resource name with maximum of 18 characters.
   * Remember the base resource name that you selected. We will need it later.

6. Update the following fields in the template:

    i. **App Client ID**: The application (client) ID of the Microsoft Teams Approvals Hub tab application. (from Step 1)
    
    ii. **App Client Secret**: The client secret of the Microsoft Teams Approvals Hub tab app. (from Step 1)
    
    iii. **Agent Client ID**: The application (client) ID of the Microsoft Teams Agent bot app. (from Step 1)
    
    iv. **Agent Client Secret**: The client secret of the Microsoft Teams Agent bot app. (from Step 1)
    
    v. **Service Now Base URL**: The URL of the Service Now instance, e.g.,
    https://dev52638.service-now.com/.
    vi. **Service Now Username**: The username of the Service Now instance, e.g., admin.
    
    vii. **Service Now Password**: The password of the Service Now user.
    
    viii. **Service Now Client ID**:The client ID of the Service Now app, e.g.,
    "0957207b89261110bc164aab6e288e80".
    
    ix. **Service Now Client Secret**: The client secret of the Service Now app.
    
    x. **Service Now Use Mock Service**: Inform True in case you want to mock the Service Now REST API data.
    
    xi. **Approvals Hub Bot Notification Url**: The URL for the Approvals Hub Agent bot.
    
7. **User App ExternalId [Optional]**: Default value is `fb65077d-ee8e-4755-a0fa-e646e0109484`. This **MUST** be the same `id` that is in the Teams app manifest for the user app.
    
        > **Note:** Make sure to keep the same values for an upgrade.

8. Click on **Review + create** button to start the validation process.

![](Images/review%20create.png)

9. Make sure that the validation passed. Click on Create to agree with the Azure terms and conditions and to start the deployment.

![](Images/create%20deployment.png)

10. Wait for the deployment to finish.

![](Images/deployment%20in%20progress.png)

You can also check the progress of the deployment from the "Notifications" pane of the Azure Portal. It may take **up to half hour** for the deployment to finish.

![](Images/deployment%20completed.png)

11. Make sure that the deployment is successfully completed, and click on **Go to resource group**.


## 3. Set-up Authentication

### Approvals Hub Tab

1. Go to **App Registrations** page [here](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/RegisteredApps) and open the **Approvals Hub Tab**  app you created (in Step 1) from the application list.

2. Under **Manage**, click on **Authentication** to bring up authentication settings.

i. Click on **Add a platform** and then on **Web**:

![](Images/add%20platform%20for%20redirect%20web.png)

ii. Add a new entry to **Redirect URIs**: Enter `https://%appDomain%/auth-end.html` for the URL e.g. `https://opapp01webapp.azurewebsites.net/auth-end.html`.

iii. Click **Configure** to commit your changes.

![](Images/configure%20auth.png)
    
iv. Click again on **Add a platform** and then on **Single-page application**:

![](Images/add%20singlepage%20app.png)

v. Add a new entry to **Redirect URIs**: Enter `https://%appDomain%/auth-end.html?clientId=%appClientId%` for the URL e.g. `https://opapp01webapp.azurewebsites.net/auth-end.html?clientId=d9446edc-6c52-4c38-968c-443a2c12535e.

![](Images/configure%20single%20app.png)

vi. Click **Configure** to commit your changes.

vii. Click on Add URI to add the `https://%appDomain%/blank-auth-end.html` for the URL e.g. `https://opapp01webapp.azurewebsites.net/blank-auth-end.html`.
    
![](Images/save%20auth.png)
    
viii. Click **Save** to commit your changes.
    
3. Back under Manage, click on **Expose an API**.
    
i. Click on the Set link next to Application ID URI:
        
![](Images/expose%20api%20set.png)
        
ii. Change the value to **api://%appDomain%/appclientid** e.g. **api://opapp01webapp.azurewebsites.net/7b9416fc-1ad8-4f1a-a132-86672112d78f**.
      
  ![](Images/expose%20api%20set%20save.png)
        
iii. Click Save to commit your changes.
    
iv. Click on Add a scope, under Scopes defined by this API. In the flyout that appears, enter the following values:
    
* **Scope name:** access_as_user
* **Who can consent?:** Admins and users
* **Admin and user consent display name:** Teams can access app’s web APIs
* **Admin and user consent description:** Allows Teams to call the app’s web APIs as the current user.
* **User consent display name:** Teams can access app’s web APIs and make requests on your behalf
* **User consent description:** Enable Teams to call this app’s web APIs with the same rights that you have

v. Click **Add scope** to commit your changes.
        
![](Images/add%20scope.png)
        
vi. Click **Add a client application**, under **Authorized client applications**. In the flyout that appears, enter the following values:

* **Client ID**: `5e3ce6c0-2b1f-4285-8d4b-75ee78787346`
* **Authorized scopes**: Select the scope that ends with `access_as_user`. (There should only be 1 scope in this list.)
* Click **Add application** to commit your changes.

* **Client ID**: `1fec8e78-bce4-4aaf-ab1b-5451cc387264`
* **Authorized scopes**: Select the scope that ends with `access_as_user`. (There should only be 1 scope in this list.)

* Click **Add application** to commit your changes.

 ![](Images/expose%20api%20add%20clientid.png)
 
 
### Approvals Hub M365
 
1. Go to **App Registrations** page [here](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/RegisteredApps) and open the **Approvals Hub Tab**  app you created (in Step 1) from the application list.

2. Under **Manage**, click on **Authentication** to bring up authentication settings.

i. Click on **Add a platform** and then on **Web**:

![](Images/add%20web%20plat%20m36.png)

ii. Add the URL of the **Approvals Hub Agent** App Service  to **Redirect URIs**: Enter `https://%appDomain%/auth-end.html` for the URL e.g. `https://opapp01agent.azurewebsites.net/auth-end.html`.

iii. Click **Configure** to commit your changes.

![](Images/configure%20auth%20agent.png)
    

3. Back under Manage, click on **Expose an API**.
    
i. Click on the Set link next to Application ID URI:
        
![](Images/expose%20api%20set%20m365.png)
        
ii. Change the value to **api://botid-%botid%** e.g. **api://botid-fa7ebe36-9763-48a7-82a7-f669f844556d**.

  ![](Images/expose%20api%20set%20save%20m365.png)

iii. Click Save to commit your changes.

You can find the bot id on:

![](Images/botid%20agent.png)

iv. Click on Add a scope, under Scopes defined by this API. In the flyout that appears, enter the following values:
    
* **Scope name:** access_as_user
* **Who can consent?:** Admins and users
* **Admin and user consent display name:** Teams can access app’s web APIs
* **Admin and user consent description:** Allows Teams to call the app’s web APIs as the current user.
* **User consent display name:** Teams can access app’s web APIs and make requests on your behalf
* **User consent description:** Enable Teams to call this app’s web APIs with the same rights that you have

v. Click **Add scope** to commit your changes.
        
![](Images/add%20scope%20m365.png)
        
vi. Click **Add a client application**, under **Authorized client applications**. In the flyout that appears, enter the following values (please use the same values as below):

* **Client ID**: `5e3ce6c0-2b1f-4285-8d4b-75ee78787346` (Teams web application)
* **Authorized scopes**: Select the scope that ends with `access_as_user`. (There should only be 1 scope in this list.)
* Click **Add application** to commit your changes.

![](Images/expose%20api%20add%20clientid%20%01%20m365.png)

* **Client ID**: `1fec8e78-bce4-4aaf-ab1b-5451cc387264` (Teams mobile/desktop application)
* **Authorized scopes**: Select the scope that ends with `access_as_user`. (There should only be 1 scope in this list.)
* Click **Add application** to commit your changes.

![](Images/expose%20api%20add%20clientid%2002%20m365.png)
 
 vii. Select **Token configuration** blade from the left hand side.
 
 viii. Click on **Add option claim**, select the token type Access, select the idtyp claim and click on Add.
 
 
 ![](Images/optional%20claim.png)
 
 
 The additional claim idtyp to let the application needs to distinguish between app-only access tokens and access tokens for users.
     
## 4. Add Permissions to your app

### Approvals Hub Tab

Continuing from the Approvals Hub app registration page where we ended Step 4.

1. Select **API Permissions** blade from the left hand side.

2. Click on **Add a permission** button to add permission to your app.

3. Under "Commonly used Microsoft APIs", select **Microsoft Graph**:

![](Images/add%20api%20permissions.png)
    
* Select **Delegated permissions** and check the following permissions:

   - **Directory.Read.All**
   - **User.Read.All**
   - **User.ReadBasic.All**
    
* Click on **Add Permissions** to commit your changes.
    
    ![](Images/delegated%20permissions.png)    


## 5. Create the Teams app packages

Approvals Hub app comes with 2 applications – **Tab application** and **Agent Notification**. The Tab application is intended for employees with the access to approve or reject tickets in the organization, and the Agent notification application is intended for employees who want to receive the messages about the approval status.

Make sure you have cloned the app repository locally.

### Create the Teams Tab package

1. Copy the `MyApprovalsHub\Templates\appPackage\manifest.template.json` and the images from the resources folder to another folder.

![](Images/manifest%20folder.png)

2. Rename the manifest.template.json file to manifest.json.

3. Open the manifest.json file and change the placeholder `{{state.fx-resource-appstudio.teamsAppId}}` with the value of teamsAppId that exists on **\MyApprovalsHub\MyApprovalsHub\.fx\states\state.dev.json**, e.g.,**fb65077d-ee8e-4755-a0fa-e646e0109484**.


4. Change the placeholder fields in the manifest to values appropriate for your organization.
    * `developer.name` ([What's this?](https://docs.microsoft.com/en-us/microsoftteams/platform/resources/schema/manifest-schema#developer))
    * `developer.websiteUrl`
    * `developer.privacyUrl`
    * `developer.termsOfUseUrl`

5. Remove the folder name from the icons path, since the images are in the root now.

```json
"icons": {
        "color": "color.png",
        "outline": "outline.png"
    },
```
> Please notice that the name of the images files is case-sensitive.

6. Provide a description of your preference for the name and description sessions.

```json
    "name": {
        "short": "Approvals Hub Agent",
        "full": "Full name for MyApprovalsHub Agent bot"
    },
    "description": {
        "short": "Short description of MyApprovalsHub.Agent",
        "full": "Full description of MyApprovalsHub Agent bot"
    },
```

7. Change the `{{state.fx-resource-frontend-hosting.endpoint}}{{state.fx-resource-frontend-hosting.indexPath}}` placeholder in the **configurationUrl** setting to be the `%appDomain%` value e.g. "`https://opapp01webapp.azurewebsites.net/configtab`".

8. Change the `{{state.fx-resource-frontend-hosting.endpoint}}{{state.fx-resource-frontend-hosting.indexPath}}` placeholder in the **contentUrl** setting to be the `%appDomain%` value e.g. "`https://opapp01webapp.azurewebsites.net/tab`".

9. Change the `{{state.fx-resource-frontend-hosting.endpoint}}{{state.fx-resource-frontend-hosting.indexPath}}` placeholder in the **websiteUrl** setting to be the `%appDomain%` value e.g. "`https://opapp01webapp.azurewebsites.net/tab`".

10. Add the validDomains setting to be the `%appDomain%` value e.g. "`https://opapp01webapp.azurewebsites.net`".

11. Change the `{{state.fx-resource-aad-app-for-teams.clientId}}` placeholder in the resource setting of webApplicationInfo section to be the `%appClientId%` value - this is your Approvals Hub AD application's ID created in the first step, e.g. **d9446edc-6c52-4c38-968c-443a2c12535e**.

12. Change the `{{state.fx-resource-aad-app-for-teams.applicationIdUris}}` placeholder in the resource setting of webApplicationInfo section to be the `api://%appdomain%/%appClientId%` value, e.g. **api://opapp01webapp.azurewebsites.net/d9446edc-6c52-4c38-968c-443a2c12535e**.

13. Save the `manifest.json`.

Follows my sample:

```json
{
    "$schema": "https://developer.microsoft.com/en-us/json-schemas/teams/v1.13/MicrosoftTeams.schema.json",
    "manifestVersion": "1.13",
    "version": "1.0.0",
    "id": "fb65077d-ee8e-4755-a0fa-e646e0109484",
    "packageName": "com.microsoft.teams.approvals.tabapp",
    "developer": {
        "name": "Luis Demetrio",
        "websiteUrl": "https://github.com/luishdemetrio/MyApprovalsHub",
        "privacyUrl": "https://github.com/luishdemetrio/MyApprovalsHub",
        "termsOfUseUrl": "https://github.com/luishdemetrio/MyApprovalsHub"
    },
    "icons": {
        "color": "color.png",
        "outline": "outline.png"
    },
    "name": {
        "short": "Approvals Hub",
        "full": "Developed to aggregate approvals from the identified outsides systems"
    },
    "description": {
        "short": "Approvals Hub",
        "full": "Developed to aggregate approvals from the identified outsides systems"
    },
    "accentColor": "#FFFFFF",
    "bots": [],
    "composeExtensions": [],
    "configurableTabs": [
        {
            "configurationUrl": "https://opapp01webapp.azurewebsites.net/config",
            "canUpdateConfiguration": true,
            "scopes": [
                "team",
                "groupchat"
            ]
        }
    ],
    "staticTabs": [
        {
            "entityId": "index",
            "name": "Personal Tab",
            "contentUrl": "https://opapp01webapp.azurewebsites.net/tab",
            "websiteUrl": "https://opapp01webapp.azurewebsites.net/tab",
            "scopes": [
                "personal"
            ]
        }
    ],
    "permissions": [
        "identity",
        "messageTeamMembers"
    ],
    "validDomains": [
        "https://opapp01webapp.azurewebsites.net"
    ],
    "webApplicationInfo": {
        "id": "d9446edc-6c52-4c38-968c-443a2c12535e",
        "resource": "api://opapp01webapp.azurewebsites.net/d9446edc-6c52-4c38-968c-443a2c12535e"
    }
}
```


15. Create a ZIP package with the `manifest.json`,`color.png`, and `outline.png`. The two image files are the icons for your app in Teams.
    * Name this package `approvalshubapp.zip`, so you know that this is the app for the Approval Hub tab.
    * Make sure that the 3 files are the _top level_ of the ZIP package, with no nested folders. 
    
![](Images/zipfile.png)


### Create the Teams Agent package

1. Copy the `MyApprovalsHub.Notification\Templates\appPackage\manifest.template.json` and the images from the resources folder to another folder.

![](Images/agent%20manifest%20folder.png)

2. Rename the manifest.template.json file to manifest.json.

3. Open the manifest.json file and change the placeholder `{{state.fx-resource-appstudio.teamsAppId}}` with the value of teamsAppId that exists on **\MyApprovalsHub\MyApprovalsHub\.fx\states\state.dev.json**, e.g., **c34c9482-7370-40a8-b169-a271a31eb772**.

4. Change the placeholder fields in the manifest to values appropriate for your organization.

    * `developer.name` ([What's this?](https://docs.microsoft.com/en-us/microsoftteams/platform/resources/schema/manifest-schema#developer))
    * `developer.websiteUrl`
    * `developer.privacyUrl`
    * `developer.termsOfUseUrl`

5. Remove the folder name from the icons path, since the images are in the root now.

```json
"icons": {
        "color": "color.png",
        "outline": "outline.png"
    },
```

6. Provide a description of your preference for the name and description sessions.

```json
    "name": {
        "short": "MyApprovalsHub.Agent",
        "full": "Full name for MyApprovalsHub Agent bot"
    },
    "description": {
        "short": "Short description of MyApprovalsHub.Agent",
        "full": "Full description of MyApprovalsHub Agent bot"
    },
```

7. Change the `{{state.fx-resource-bot.botId}}` placeholder in the **botId** setting to be the client secret of the Microsoft Teams Agent bot app created on the step 1, e.g. "`fa7ebe36-9763-48a7-82a7-f669f844556d`".

8. Add the validDomains setting to be the `%appDomain%` value of your bot web application, e.g. "`https://opapp01agent.azurewebsites.net`".

9. Change the `{{state.fx-resource-aad-app-for-teams.clientId}}` placeholder in the resource setting of webApplicationInfo section to be the `M365 Client Secret` value - this is your Approvals Hub Agent AD application's ID created in the first step, e.g. **dee04302-5457-4446-af86-d1d9a19d3ecd**.

10. Change the `{{state.fx-resource-aad-app-for-teams.applicationIdUris}}` placeholder in the resource setting of webApplicationInfo section to be the `api://botid-%botId%` setting to be the client secret of the Microsoft Teams Agent bot app (the same one used on the item 5), e.g. **api://botid-fa7ebe36-9763-48a7-82a7-f669f844556d**.

11. Save the `manifest.json`.

Follows my sample:

```json
{
    "$schema": "https://developer.microsoft.com/en-us/json-schemas/teams/v1.13/MicrosoftTeams.schema.json",
    "manifestVersion": "1.13",
    "version": "1.0.0",
    "id": "{{state.fx-resource-appstudio.teamsAppId}}",
    "packageName": "com.microsoft.teams.extension",
    "developer": {
        "name": "Luis Demetrio",
        "websiteUrl": "https://github.com/luishdemetrio/MyApprovalsHub",
        "privacyUrl": "https://github.com/luishdemetrio/MyApprovalsHub",
        "termsOfUseUrl": "https://github.com/luishdemetrio/MyApprovalsHub"
    },
    "icons": {
        "color": "color.png",
        "outline": "outline.png"
    },
    "name": {
        "short": "Approvals Hub",
        "full": "Developed to aggregate approvals from the identified outsides systems"
    },
    "description": {
        "short": "Approvals Hub",
        "full": "Developed to aggregate approvals from the identified outsides systems"
    },
    "accentColor": "#FFFFFF",
    "bots": [
        {
            "botId": "fa7ebe36-9763-48a7-82a7-f669f844556d",
            "scopes": [
                "personal",
                "team",
                "groupchat"
            ],
            "supportsFiles": false,
            "isNotificationOnly": false
        }
    ],
    "composeExtensions": [],
    "configurableTabs": [],
    "staticTabs": [],
    "permissions": [
        "identity",
        "messageTeamMembers"
    ],
    "validDomains": [
        "https://opapp01agent.azurewebsites.net"
    ],
    "webApplicationInfo": {
        "id": "dee04302-5457-4446-af86-d1d9a19d3ecd",
        "resource": "api://botid-fa7ebe36-9763-48a7-82a7-f669f844556d"
    }
}
```


12. Create a ZIP package with the `manifest.json`,`color.png`, and `outline.png`. The two image files are the icons for your app in Teams.
    * Name this package `approvalshubapp.zip`, so you know that this is the app for the Approval Hub tab.
    * Make sure that the 3 files are the _top level_ of the ZIP package, with no nested folders. 
    
![](Images/agentzipfile.png)

## 6. Install the apps in Microsoft Teams
    
### Install the Approvals Hub Tab app
    
1. Open Microsoft Teams, click on Apps, manage your Apps and Upload an App.

![](Images/add%20custom%20app.png)

For more information, please check this post [https://learn.microsoft.com/en-us/microsoftteams/upload-custom-apps](https://learn.microsoft.com/microsoftteams/upload-custom-apps)

2. Click on the **Upload a custom app** item from the flyout that will appear:

![](Images/upload%20a%20custom%20app.png)

3. Select the ZIP package that you have created in the previous step:

![](Images/add%20zip%20package.png)

4. Click on Add:

![](Images/add%20app%20to%20teams.png)
    
5. You should have the following UI:

![](Images/app%20running.png)


### Install the Approvals Hub Agent Bot 
    
1. Open Microsoft Teams, click on Apps, manage your Apps and Upload an App.

![](Images/add%20custom%20app.png)

For more information, please check this post [https://learn.microsoft.com/en-us/microsoftteams/upload-custom-apps](https://learn.microsoft.com/microsoftteams/upload-custom-apps)

2. Click on the **Upload a custom app** item from the flyout that will appear:

![](Images/upload%20a%20custom%20app.png)

3. Select the ZIP package that you have created in the previous step:


![](Images/agentmanifest.png)

4. Click on Add:

![](Images/addagentbot.png)



