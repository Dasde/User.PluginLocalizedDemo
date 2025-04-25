# SimHub Localization plugin + Base Localization plugin project 1.0 #
For a project I'm working on I wanted to localize a Simhub plugin.  
I found the task not as trivial as I would have thought so I decided to share a basic plugin project already localization ready.  
In order to create only one .dll file and embed all localization .dll file I used Resource.Embedder https://www.nuget.org/packages/Resource.Embedder/2.2.0?_src=template

> [!IMPORTANT]
> The main ressources file must have the Custom Tool set PublicResXFileCodeGenerator !! (properies tab)
> 
For dashboard creator I also provide a simple plugin that exposes the current language selected in SimHub settings as 2 properties :
- Code
- Language

![Plugin properties](https://www.overtake.gg/attachments/localizationdetector-png.834458/)
