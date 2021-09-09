Module Control_List
    'List of user controls that connect with PLC
    Public Enum PlcControlTypes
        VS7_Textbox
        VS7_ValueLabel
        VS7_Led
        VS7_Checkbox
        VS7_Button
        VS7_ComboBox
        VS7_RadioButton
        VS7_PictureBox
        VS7_IOByte
        VS7_Register
        VS7_RWVariable
        ViSnap7_DynamicLabel
        VS7_Trends
        VS7_Gauge
        VS7_Slider
        VS7_ListBox
        VS7_HScrollBar
        VS7_VScrollBar
    End Enum
    ' List of user controls that be used in forms
    ' After configure the control, it must be added to this Enum. 
    Public Enum PlcCrtCanBeForms
        VS7_Textbox
        VS7_Checkbox
        VS7_ComboBox
        VS7_RadioButton
        VS7_HScrollBar
        VS7_VScrollBar
        VS7_Slider
    End Enum
End Module
