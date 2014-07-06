#Change log


####Version 0.1.0
- [x] Added "LabeledConectionLine" control which position is updated during "DesignerItem" movement.
- [x] Added "ConnectionProperties" class holding all model information about connection
- [x] Added "ModelItem" classes which represents the actual model nodes behind the "DesignerItems"
- [x] Added "DecisionModel", "ChanceModel", "EndModel" classes which inherits after the "ModelItem" class
- [x] Added "PropertiesBox" control to maintain node and its connections data
- [x] Added "ConnetionPropertiesBox" control which is dynamical added to "PropertiesBox" based on number of base node connections
- [x] Performed a data binding between a "LabeledConnectionLine" and a adequate "ConnectionProperties"
- [x] Performed a data binding between a "PropertiesBox" and a selected "DesignerItem" as a "ModelItem"
- [x] Performed a data binding between a "ConnectionPropertiesBox" and a "ConnetionProperties" from the selected ModelItem
- [x] Much more visual tweaking

####Version 0.0.2
- [x] Customizable grid-like designer items behaviour.
- [x] Removed "ResizeControlDecorator"
- [x] Added "SelectionDecorator" instead of "ResizeControlDecorator"
- [x] Modified temporary connection behaviour. Now it's showing the connection after appropriate horizontal distance
- [x] Added "ConnectionDecorator" class which handles "ConnectionLine" decorator during drag&drop operations
- [x] Moved "RubberbandSelection" logic almost completely to RubberbandSelection class
- [x] Added "Path" control to "ConnectionLine" control, which imitates arrow's head
- [x] Add Overrode Margin Property to "ConnectionLine" class. This enables setting internal margin for the connection lines
- [x] Dropping a ToolboxItem on the DesignerGrid with a visible connection decorator creates a connection line between closest "DesignerItems".