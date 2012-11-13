The Move Validator makes sure that when you create an item under another item, the item 
gets validated so that you are only allowed to insert items from a template that is specified 
in the assign options.

Shared Source Site: http://trac.sitecore.net/MoveValidator

Installation Instructions - The first configuration change that will need to be made is to add the 
following lines to the web.config file. 

Please add these entries above the execute command within the specified processors section, e.g. <uiCopyItems>:

	uiCopyItems:	<processor mode="on" type="Velir.SitecoreLibrary.Modules.MoveValidator.CustomSitecore.Pipeline.CustomCopyItems,Velir.SitecoreLibrary.Modules.MoveValidator" method="ConstrainMove" />
	uiDragItemTo:	<processor mode="on" type="Velir.SitecoreLibrary.Modules.MoveValidator.CustomSitecore.Pipeline.CustomDragItemTo,Velir.SitecoreLibrary.Modules.MoveValidator" method="ConstrainDragTo" />
	uiMoveItmes:	<processor mode="on" type="Velir.SitecoreLibrary.Modules.MoveValidator.CustomSitecore.Pipeline.CustomMoveItems,Velir.SitecoreLibrary.Modules.MoveValidator" method="ConstrainMove" />

Then you will need to adjust the commands.config file found here ~/App_Config/Commands.config. Find the command for: item:pastefromclipboard. You want to 
replace it with the following command.

	<command name="item:pastefromclipboard" type="Velir.SitecoreLibrary.Modules.MoveValidator.CustomSitecore.Commands.CustomPasteFromClipBoard,Velir.SitecoreLibrary.Modules.MoveValidator" />