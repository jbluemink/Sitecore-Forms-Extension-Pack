# Sitecore-Forms-Extension-Pack
The current Sitecore Forms for Sitecore 9 is missing a lot of basic functionality with this extension pack you get extra validations, save actions and advanced fields.

## Cloud
Sitecore Forms and Cloud
Azure cloud solutions offer scalability and reliability with Sitecore Forms you can also easily use advanced cloud solutions. The first thing you need is a save action that puts a message in a queue. you can go further from there.
See also the new version with support for Sitecore 9.3 file uploads. http://www.stockpick.nl/english/sitecore-93-forms-process-sensitive-files/

## Send E-mail Action
The Send E-mail Action is a port of the existent action hosted on https://bitbucket.org/nishtechinc/formssendmail
Please refer to the original documentation about this action at the repository and this blog post: 
http://www.nishtechinc.com/Blog/2018/April/Send-E-mail-Action-to-Sitecore-9-Forms

## Setup
Adjust the targetDataStore physicalRootPath in Unicorn.config

## Setup Upload Azure functions.
- Publish project Feature/ExpienceForms/sitecoreupload to Azure
- Configer Application settings UploadStorageAccessKey to an Azure Blob storage
- Add your Sitecore hostnames to the Azure function CORS.
- Set your url to the Azure function in \Features\ExperienceForms\code\Views\FormBuilder\FormsExtensionPack\Upload.cshtml
