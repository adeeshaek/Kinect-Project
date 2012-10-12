/*
 * myGUIControls
 * purpose: define custom GUI controls, such as
 *  dialogBox
 * author: Adeesha Ekanayake
 */

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; //for list struc
using System.IO; //for searching through filesystem

///<summary>
///Class for defining custom GUI controls for use in Unity:
///InputBox, SimpleList, GUIList, SaveDialog, ButtonPanel
///Author: Adeesha Ekanayake
///Date: July 2012
///</summary>
public class myGUIControls : MonoBehaviour {

    /*
     * Class definitions
     */
    /// <summary>
    /// Class encapsulating variables for implementing a GUI textbox in Unity
    /// </summary>
    [System.Serializable]
    public class IBoxTextField
    {
        /// <summary>
        /// Defines the dimensions of the control
        /// </summary>
        public int X, Y, width, height, spacing;
        /// <summary>
        /// Text displayed in the text field
        /// </summary>
        public string text;
    };

    /// <summary>
    /// Class encapsulating variables for implementing a GUI label in Unity
    /// </summary>
    [System.Serializable]
    public class IBoxLabel
    {
        /// <summary>
        /// Dimensions of the control
        /// </summary>
        public int X, Y, width, height;
        /// <summary>
        /// Text of the label
        /// </summary>
        public string text;
    };

    /// <summary>
    /// Class encapsulating variables for implementing a GUI button in Unity
    /// </summary>
    [System.Serializable]
    public class IBoxButton
    {
        /// <summary>
        /// Dimensions of the control
        /// </summary>
        public int X, Y, width, height, spacing;
        /// <summary>
        /// Text displayed on the button
        /// </summary>
        public string text;
    };

    /// <summary>
    /// Class encapsulating variables for implementing GUI slider in Unity
    /// </summary>
    [System.Serializable]
    public class IBoxSlider
    {
        /// <summary>
        /// Dimensions of the control
        /// </summary>
        public int X, Y, width, height, size;
        /// <summary>
        /// Value of the slider position
        /// </summary>
        public float value;
    };


    /// <summary>
    /// Defines Event handler for event raised when slider is moved around
    /// </summary>
    /// <param name="sender">Object invoking event</param>
    /// <param name="e">Arguments</param>
    public delegate void IBoxSliderEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Encapsulates a GUI slider control
    /// This slider's min value is fixed at 0, and max is fixed at 100
    /// </summary>
    [System.Serializable]
    public class IBoxHSlider
    {
        /// <summary>
        /// Dimensions of the slider control
        /// </summary>
        public int X, Y, width, height;

        /// <summary>
        /// The value of the slider position
        /// </summary>
        public float value;

        /// <summary>
        /// Old value of slider. Used to check if slider has moved
        /// </summary>
        private float oldValue = 0;

        /// <summary>
        /// Event handler for when value is changed
        /// </summary>
        public IBoxSliderEventHandler SliderEventHandler;

        /// <summary>
        /// Passes event raised when slider value is changed
        /// </summary>
        /// <param name="e">Event args for this event</param>
        public void OnSliderValueChange(EventArgs e)
        {
            SliderEventHandler(this, e);
        }

        /// <summary>
        /// Place in Update() method.
        /// Checks if value of slider has been changed and 
        /// calls event handler accordingly
        /// </summary>
        public void Update()
        {
            if (oldValue != value)
            {
                oldValue = value; //reset flag
                OnSliderValueChange(new EventArgs());
            }
        }

        /// <summary>
        /// Draws the control. Place in OnGUI()
        /// </summary>
        public void Draw()
        {
            value = GUI.HorizontalSlider(new Rect(X, Y, width, height),
                value, 0, 100);
        }
    }


    /// <summary>
    /// Class defining event arguments for an OkClickEvent in Unity. This event is used in the InputBox class
    /// </summary>
    public class OkClickEventArgs : EventArgs //declare params for event
    {
        private string boxType = "Load";

        //constructor
        /// <summary>
        /// Constructor method for OkClickEventArgs
        /// </summary>
        public OkClickEventArgs()
        {
            
        }

        /// <summary>
        /// The type of input box
        /// </summary>
        //properties
        public string BoxType
        {
            get { return boxType; }
            set { this.boxType = value; }
        }
    }

    /// <summary>
    /// Defines a delegate type named OkClickEventHandler, which handles the event generated by clicking the Ok button in an InputBox
    /// </summary>
    /// <param name="sender">Reference to object which invoked the event</param>
    /// <param name="e">Arguments associated with event</param>
    public delegate void OkClickEventHandler(object sender, OkClickEventArgs e); //delegate

    /// <summary>
    /// Class defining a custom GUI control, InputBox, for use in Unity
    /// InputBox is a standard Input Box implementation, with an OK button and a cancel button.
    /// </summary>
    [System.Serializable]
    public class InputBox
    {
        //abstract properties
        //public string boxType; //whether save or load dialog box

        //frame properties
        /// <summary>
        /// Text noted at the title of the InputBox
        /// </summary>
        public string titleText;
        /// <summary>
        /// Integer values used to define dimensions of inputbox
        /// </summary>
        public int X, Y, width, height;
        /// <summary>
        /// boolean which determines if box is visible or not
        /// </summary>
        public bool isVisible = false;

        //controls
        /// <summary>
        /// IboxLabel object next to the textfield
        /// </summary>
        public IBoxLabel fieldLabel;

        /// <summary>
        /// textField object for input
        /// </summary>
        public IBoxTextField textField;

        /// <summary>
        /// 2 button objects; OkButton and CancelButton
        /// </summary>
        public IBoxButton okButton, cancelButton;

        //event handler
        /// <summary>
        /// Event handler for the event raised when user clicks 'Ok'
        /// </summary>
        public event OkClickEventHandler OkClick;

        /// <summary>
        /// Method to make input box appear. 
        /// Note that this is not a constructor; the object should already exist for method to be invoked
        /// </summary>
        /// <param name="titleTextIn">Title of the inputBox</param>
        /// <param name="labelTextIn">Text of the label next to the field</param>
        /// <param name="okButtonTextIn">Text of ok button</param>
        /// <param name="cancelButtonTextIn">text of Cancel Button</param>
        /// <param name="handlerIn">Event handler for Ok click event</param>
        public void prompt(string titleTextIn, string labelTextIn,
            string okButtonTextIn, string cancelButtonTextIn, OkClickEventHandler handlerIn)
        {
            //set up variables 
            titleText = titleTextIn;
            fieldLabel.text = labelTextIn;
            okButton.text = okButtonTextIn;
            cancelButton.text = cancelButtonTextIn;
            OkClick = handlerIn;

            //make visible
            isVisible = true;
        }

        /// <summary>
        /// Event handler for Ok click event.
        /// This method passes event to true event handler
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnOkClick(OkClickEventArgs e)
        {
            isVisible = false; //set to invisible
            OkClick(this, e); //invoke true click event
        }

        /// <summary>
        /// Event handler for Cancel click event
        /// No true event handler
        /// </summary>
        public void CancelClickEvent()
        {
            isVisible = false; //set to invisible
        }

        /// <summary>
        /// Method which draws all the controls. 
        /// NOTE: Call this method in OnGUI()
        /// </summary>
        public void Draw() //call in OnGUI
        {
            if (isVisible) //only draw when visible
            {
                GUI.Box(new Rect(X, Y, width, height), titleText); //draw box

                textField.text  = GUI.TextField(new Rect(X + textField.X, Y + textField.Y, //draw textField
                    textField.width, textField.height), textField.text);

                GUI.Label(new Rect(X + fieldLabel.X, Y + fieldLabel.Y, //draw label
                    fieldLabel.width, fieldLabel.height), fieldLabel.text);

                if(GUI.Button(new Rect(X + okButton.X, Y + okButton.Y, //ok button
                    okButton.width, okButton.height), okButton.text))
                    OnOkClick(new OkClickEventArgs());

                if(GUI.Button(new Rect(X + cancelButton.X, Y + cancelButton.Y, //cancel button
                    cancelButton.width, cancelButton.height), cancelButton.text))
                    isVisible = false; 

            }
        }

    };

    /// <summary>
    /// Defining event handler for GUI list.
    /// Handles event raised when item in list is selected
    /// </summary>
    /// <param name="sender">reference to object raising handler</param>
    /// <param name="e">Event arguments</param>
    public delegate void ListItemSelectedEventHandler(object sender, EventArgs e); //delegate

    /*
     * class SimpleList
     * purpose: a GUI control list which can be used to build
     *  up more complex GUI controls
     */
    /// <summary>
    /// A GUI control for Unity, which is a simple list control
    /// Consists of a list of buttons and a vslider
    /// This control does not display the list element selected in any way
    /// </summary>
    [System.Serializable]
    public class SimpleList
    {

        //size parameters
        /// <summary>
        /// Parameters defining dimensions of object
        /// </summary>
        public float X, Y, width, height;

        /// <summary>
        /// Offset for slider
        /// </summary>
        private float sliderOffset;

        //output parameter
        /// <summary>
        /// Output of list: index of list element selected
        /// </summary>
        public int outputIndex;

        //title text
        /// <summary>
        /// Title of the Control. 
        /// Probably should not be used; does not display in a pleasing way
        /// </summary>
        public string titleString;

        //controls
        /// <summary>
        /// the list of buttons in the 'list'. 
        /// Only the text of the list buttons is taken into account
        /// </summary>
        private List<IBoxButton> buttonList = new List<IBoxButton>();
        /// <summary>
        /// This defines the dimensions of each element drawn in the list
        /// </summary>
        public IBoxButton buttonArrayCast; //used to draw the buttons
        /// <summary>
        /// Defines the vertical slider used in the list control
        /// </summary>
        public IBoxSlider vSlider;

        //event handler
        /// <summary>
        /// Event handler. 
        /// Handles event raised when user selects an item in the list
        /// </summary>
        public event ListItemSelectedEventHandler listItemSelectedEventHandler;

        /// <summary>
        /// Event handler for when the user selects an item in the list
        /// </summary>
        /// <param name="e">Event arguments</param>
        /// <param name="buttonIndex">Index of button which is selected</param>
        protected virtual void ListItemSelected(EventArgs e, int buttonIndex)
        {
            outputIndex = buttonIndex; //make selected button's index appear
            listItemSelectedEventHandler(this, e); // call true event handler
        }

        /// <summary>
        /// Method which draws the GUI controls
        /// NOTE: call this method in OnGUI()
        /// </summary>
        public void Draw() //draw method (place in OnGUI()
        {
            //draw outer rectangle
            GUI.Box(new Rect(X, Y, width, height), titleString);

            //draw slider
            vSlider.value = GUI.VerticalScrollbar
                (new Rect(vSlider.X + X, vSlider.Y + Y, width - vSlider.width, height - vSlider.height),
                vSlider.value, vSlider.size, 0, 100); //hardcoded btw 0 and 100

            //draw button array
            for (int i = 0; i < buttonList.Count; i++)
            {
                
                //display button only if it is within the rectangle
                if ((buttonArrayCast.Y + Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing) - sliderOffset)
                    > Y)
                    &&
                    ((buttonArrayCast.Y + Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing) - sliderOffset)
                    + buttonArrayCast.height) < (Y + height))) 
                {
                    if (GUI.Button
                        (new Rect(buttonArrayCast.X + X,
                            buttonArrayCast.Y + Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing) - sliderOffset),
                            width - buttonArrayCast.width, buttonArrayCast.height),
                            buttonList[i].text))
                    {
                        ListItemSelected(new EventArgs(), i); //Event Handler
                    }
                }
               
            }

        }

        /// <summary>
        /// Method to add element to the list.
        /// </summary>
        /// <param name="textIn">The text of the list item added</param>
        public void add(string textIn)
        {
            //define new item
            IBoxButton newButton = new IBoxButton();
            newButton.text = textIn;

            buttonList.Add(newButton); //add new button to list
        }

        /// <summary>
        /// Method used to refresh the contents of the list control.
        /// Note: invoke this method from the Update() method
        /// </summary>
        public void UpdateMethod()
        {
            //calculate size of slider
            //size  = virtual height / window height.
            ResizeSlider();
            ChangeOffset();
        }

        /// <summary>
        /// Method used to resize slider control
        /// Keeps slider sized proportionately to contents of list
        /// </summary>
        public void ResizeSlider()
        {
            //calculate size of slider
            //size  = virtual height / window height.
            float virtualHeight = 
                ((buttonArrayCast.height + buttonArrayCast.spacing) * buttonList.Count);
            float windowHeight = height;

            if (virtualHeight > height) //make sure slider isn't too big
            {
                vSlider.size = (int) (100 * windowHeight / virtualHeight);
            }

            else
                vSlider.size = vSlider.height;

            //Debug.Log("Height = " + windowHeight + "virtualH : " + virtualHeight);
            //Debug.Log(windowHeight / virtualHeight);
        }

        /// <summary>
        /// Used to change the offset of the list, so that the slider can be
        /// used to view all the list elements
        /// </summary>
        public void ChangeOffset()
        {
            //variables
            float virtualHeight = 
                ((buttonArrayCast.height + buttonArrayCast.spacing) * buttonList.Count) + Y;
            sliderOffset = vSlider.value * virtualHeight / 100;

        }
    }

    /// <summary>
    /// Event handler for when item in the GUI list is selected
    /// </summary>
    /// <param name="sender">reference to object calling method</param>
    /// <param name="e">Event arguments</param>
    public delegate void GUIListItemSelectedEventHandler(object sender, EventArgs e); //delegate

    /*
     * class GUIList
     * purpose: a fully-functional GUI control List
     */
    /// <summary>
    /// A fully functional GUI control list for Unity3d
    /// This control includes a button control which displays the list element selected
    /// </summary>
    [System.Serializable]
    public class GUIList
    {

        //size parameters
        /// <summary>
        /// Parameters determining the dimensions of the object
        /// </summary>
        public float X, Y, width, height;
        /// <summary>
        /// Variable to make the vSlider work
        /// </summary>
        private float sliderOffset;

        //output parameter
        /// <summary>
        /// The index of the currently selected list element. 
        /// Used for output
        /// </summary>
        public int outputIndex;

        //title text
        /// <summary>
        /// Title string for the control.
        /// NOTE: This does not draw well, so it is recommended to avoid using it
        /// </summary>
        public string titleString;

        //controls
        /// <summary>
        /// A list containing all the button elements listed out in the control
        /// Only the text of each element is used: the dimensions of each button are not used 
        /// </summary>
        private List<IBoxButton> buttonList = new List<IBoxButton>();
        /// <summary>
        /// A button element which defines the dimensions of each of the buttons in the buttonList
        /// </summary>
        public IBoxButton buttonArrayCast; //used to draw the buttons
        /// <summary>
        /// A button element displaying above the list which displays the selected element
        /// </summary>
        public IBoxButton selectedButton; //button displaying selected item
        /// <summary>
        /// A vertical slider used to manipulate the list
        /// </summary>
        public IBoxSlider vSlider;

        //event handler
        /// <summary>
        /// Defines type of Event handler for the event raised when a user selects an item in the list
        /// </summary>
        public event GUIListItemSelectedEventHandler listItemSelectedEventHandler;

        /// <summary>
        /// Event handler for event raised when user selects a list element
        /// </summary>
        /// <param name="e">Event args for event</param>
        /// <param name="buttonIndex">Index of currently selected list element</param>
        protected virtual void ListItemSelected(EventArgs e, int buttonIndex)
        {
            outputIndex = buttonIndex; //make selected button's index appear

            selectedButton.text = buttonList[buttonIndex].text + " selected";

            listItemSelectedEventHandler(this, e);
        }

        /// <summary>
        /// Method used to draw the GUI control.
        /// NOTE: Invoke this method in OnGUI()
        /// </summary>
        public void Draw() //draw method (place in OnGUI()
        {

            //draw outer rectangle
            GUI.Box(new Rect(X, Y, width, height), titleString);

            //draw slider
            vSlider.value = GUI.VerticalScrollbar
                (new Rect(X + width - vSlider.X,
                    vSlider.Y + Y + selectedButton.spacing + selectedButton.height + selectedButton.Y, 
                    vSlider.width, height - vSlider.height - selectedButton.height - selectedButton.spacing - selectedButton.Y),
                vSlider.value, vSlider.size, 0, 100); //hardcoded btw 0 and 100

            //draw selected button
            GUI.Button(new Rect(X + selectedButton.X, Y + selectedButton.Y, width - selectedButton.width,
                selectedButton.height), selectedButton.text);

            //draw button array
            for (int i = 0; i < buttonList.Count; i++)
            {

                //display button only if it is within the rectangle
                if ((buttonArrayCast.Y + Y + selectedButton.spacing/* - (selectedButton.spacing + selectedButton.height + selectedButton.Y)*/
                    + (i * (buttonArrayCast.height + buttonArrayCast.spacing) - sliderOffset)
                    > Y)
                    &&
                    ((buttonArrayCast.Y + Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing) - sliderOffset)
                    + buttonArrayCast.height) < (Y + height - ( selectedButton.spacing + selectedButton.height + selectedButton.Y))))
                {
                    if (GUI.Button
                        (new Rect(buttonArrayCast.X + X,
                            buttonArrayCast.Y + Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing)
                            + (selectedButton.Y + selectedButton.height + selectedButton.spacing ) - sliderOffset),
                            width - buttonArrayCast.width, buttonArrayCast.height),
                            buttonList[i].text))
                    {
                        ListItemSelected(new EventArgs(), i); //Event Handler
                    }
                }

            }

        }

        /// <summary>
        /// Method used to add a new element to the list
        /// </summary>
        /// <param name="textIn">The title of the element / text contained by the element</param>
        public void add(string textIn)
        {
            //define new item
            IBoxButton newButton = new IBoxButton();
            newButton.text = textIn;

            buttonList.Add(newButton); //add new button to list
        }

        /// <summary>
        /// Method used to clear the list of elements.
        /// </summary>
        public void clear()
        {
            //clear all buttons
            buttonList.Clear();
        }

        /// <summary>
        /// Method used to make the slider control work.
        /// NOTE: call this method in Update() or the slider control will not work
        /// </summary>
        public void UpdateMethod()
        {
            //calculate size of slider
            //size  = virtual height / window height.
            ResizeSlider();
            ChangeOffset();
        }

        /// <summary>
        /// Method used to make the slider control work
        /// </summary>
        public void ResizeSlider()
        {
            //calculate size of slider
            //size  = virtual height / window height.
            float virtualHeight =
                ((buttonArrayCast.height + buttonArrayCast.spacing) * buttonList.Count);
            float windowHeight = height - (selectedButton.height + selectedButton.spacing);

            if (virtualHeight > windowHeight) //make sure slider isn't too big
            {
                vSlider.size = (int)(100 * windowHeight / virtualHeight);
            }

            else
            {
                vSlider.size = 50;
            }

            //Debug.Log("Height = " + windowHeight + "virtualH : " + virtualHeight);
            //Debug.Log(windowHeight / virtualHeight);
        }

        /// <summary>
        /// method used to make the slider control work
        /// </summary>
        public void ChangeOffset()
        {
            //variables
            float virtualHeight =
                ((buttonArrayCast.height + buttonArrayCast.spacing) * buttonList.Count) + Y;
            sliderOffset = vSlider.value * virtualHeight / 100;

        }

        /// <summary>
        /// Method used to initialize the control
        /// This method sets the text of the selected button to "No item selected" upon the initialization of the Control.
        /// Note: call this method in Start() 
        /// </summary>
        public void InitMethod()
        {
            selectedButton.text = "No item selected";
        }
    }

    /// <summary>
    /// Defining the type of event handler raised when user selects and item in the list
    /// </summary>
    /// <param name="sender">a reference to the object raising the event</param>
    /// <param name="e">Event arguments</param>
    public delegate void SaveButtonEventHandler(object sender, EventArgs e); //delegate

    /*
     * class SaveDialog
     * purpose: a GUI-based save dialog box for Unity
     *  this can also be used as an open dialog box
     */
    /// <summary>
    /// A Save dialog box GUI control for Unity3d. 
    /// This can also be used as a load control.
    /// </summary>
    [System.Serializable]
    public class SaveDialog
    {
        //event parameter
        /// <summary>
        /// An enum which lets user determine if the dialogBox is used for opening or saving
        /// </summary>
        public enum eventType { Load, Save };
        /// <summary>
        /// An instance of this enum, which is a property of SaveDialog.
        /// Keeps track of purpose of dialogBox
        /// </summary>
        eventType currentEvent;

        //size parameters
        /// <summary>
        /// Parameter defining dimensions of control
        /// </summary>
        public float X, Y, width, height;
        /// <summary>
        /// Parameter to make vSlider control work
        /// </summary>
        private float sliderOffset;

        //output parameter
        /// <summary>
        /// The index of the currently selected list element. 
        /// Used for output
        /// </summary>
        public int outputIndex;
        /// <summary>
        /// The text of the currently selected list element
        /// Used for output
        /// </summary>
        public string outputText;

        //title text
        /// <summary>
        /// The title of the dialog box. 
        /// Note: This does not draw well, so I recommend not using it
        /// </summary>
        public string titleString;

        //controls
        /// <summary>
        /// List of buttons representing the elements of the list.
        /// Only the text of each button is used.
        /// The dimensions of the buttons in the list are not used
        /// </summary>
        private List<IBoxButton> buttonList = new List<IBoxButton>();
        /// <summary>
        /// Defines the dimensions of the buttons which function as list elements
        /// </summary>
        public IBoxButton buttonArrayCast; //used to draw the 
        /// <summary>
        /// A textfield which contains the filename of the selected file
        /// </summary>
        public IBoxTextField selectedField; //text field which displays selected item's filename
        /// <summary>
        /// Save and cancel buttons of the dialog box, respectively
        /// </summary>
        public IBoxButton saveButton, cancelButton; //save and cancel buttons, respectively
        /// <summary>
        /// Vertical slider control
        /// </summary>
        public IBoxSlider vSlider;

        //visibility
        /// <summary>
        /// Defines whether or not the dialog box is visible. 
        /// </summary>
        public bool isVisible = false;
        
        //event handler
        /// <summary>
        /// Handles event raised when user presses the save button
        /// </summary>
        public event SaveButtonEventHandler saveButtonEventHandler;

        /// <summary>
        /// Prompts user using the dialog box.
        /// Use this method during runtime to invoke the dialog box.
        /// </summary>
        /// <param name="loadSaveButtonText">Text of the load / save button</param>
        /// <param name="cancelButtonText">Text of the cancel button</param>
        /// <param name="eventIn">Event handler for event raised when user presses the load/save button</param>
        public void Prompt(string loadSaveButtonText, string cancelButtonText, eventType eventIn)
        {
            //change text of load/save button 
            saveButton.text = loadSaveButtonText;
            cancelButton.text = cancelButtonText;

            currentEvent = eventIn;//set event

            //make visible
            isVisible = true;
        }

        /// <summary>
        /// Returns the current event
        /// Auto-generated class? not sure what this does
        /// </summary>
        /// <returns>The current event...</returns>
        public eventType getCurrentEvent()
        {
            return currentEvent;
        }

        /// <summary>
        /// Event raised when user selects an item in the list
        /// </summary>
        /// <param name="buttonIndex">Index of currently selected list element</param>
        private void ListItemSelected(int buttonIndex)
        {
            outputIndex = buttonIndex; //make selected button's index appear

            selectedField.text = buttonList[buttonIndex].text;

            //saveButtonEventHandler(this, e);
        }

        /// <summary>
        /// Event raised when user clicks the save button
        /// </summary>
        /// <param name="e">Arguments for the event</param>
        protected virtual void SaveButtonClicked(EventArgs e)
        {
            if (selectedField.text != "No item selected")
            {
                isVisible = false; //make box invisible
                outputText = selectedField.text;
                saveButtonEventHandler(this, e);
            }

            else
                Debug.Log("enter a filename");
        }

        /// <summary>
        /// Event raised when cancel button is clicked
        /// Simply makes the dialog box invisible
        /// </summary>
        private void CancelButtonClicked()
        {
            isVisible = false; //make dialog box invisible
        }

        /// <summary>
        /// Method used to draw the GUI control.
        /// NOTE: place this in the OnGUI() method to draw the control
        /// </summary>
        public void Draw() //draw method (place in OnGUI()
        {
            if (isVisible) //only draw dialog box if isVisible is true
            {
                //draw outer rectangle
                GUI.Box(new Rect(X, Y, width, height), titleString);

                //draw slider
                vSlider.value = GUI.VerticalScrollbar
                    (new Rect(X + width - vSlider.X,
                        vSlider.Y + Y + selectedField.spacing + selectedField.height + selectedField.Y,
                        vSlider.width, height - vSlider.height - selectedField.height - selectedField.spacing - selectedField.Y
                        - saveButton.height - saveButton.spacing - saveButton.Y),
                    vSlider.value, vSlider.size, 0, 100); //hardcoded btw 0 and 100

                //draw selected text field
                selectedField.text = GUI.TextField(new Rect(X + selectedField.X, Y + selectedField.Y, width - selectedField.width,
                    selectedField.height), selectedField.text);

                //draw button array
                for (int i = 0; i < buttonList.Count; i++)
                {

                    //display button only if it is within the rectangle
                    if ((buttonArrayCast.Y + Y + selectedField.spacing/* - (selectedField.spacing + selectedField.height + selectedField.Y)*/
                        + (i * (buttonArrayCast.height + buttonArrayCast.spacing) - sliderOffset)
                        > Y)
                        &&
                        ((buttonArrayCast.Y + Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing) - sliderOffset)
                        + buttonArrayCast.height) < (Y + height - (selectedField.spacing + selectedField.height + selectedField.Y
                        + saveButton.height + saveButton.spacing + saveButton.Y))))
                    {
                        if (GUI.Button
                            (new Rect(buttonArrayCast.X + X,
                                buttonArrayCast.Y + Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing)
                                + (selectedField.Y + selectedField.height + selectedField.spacing) - sliderOffset),
                                width - buttonArrayCast.width, buttonArrayCast.height),
                                buttonList[i].text))
                        {
                            ListItemSelected(i); //Event Handler
                        }
                    }
                }




                //draw save button
                if(
                GUI.Button(
                    new Rect(X + saveButton.X,
                        Y + height - saveButton.height - saveButton.spacing,
                        saveButton.width, saveButton.height), saveButton.text))
                    SaveButtonClicked(new EventArgs());

                //draw cancel button
                if (
                GUI.Button(
                    new Rect(X + cancelButton.X,
                       Y + height - saveButton.height - saveButton.spacing,
                       cancelButton.width, saveButton.height), cancelButton.text))
                    CancelButtonClicked();
            }

        }

        /// <summary>
        /// Method used to add a new element to the list
        /// </summary>
        /// <param name="textIn">Text of the new element added</param>
        public void add(string textIn)
        {
            //define new item
            IBoxButton newButton = new IBoxButton();
            newButton.text = textIn;

            buttonList.Add(newButton); //add new button to list
        }

        /// <summary>
        /// Method used to make the slider work, as well as keep the list of files displayed up-to-date.
        /// NOTE: Call this method from Update()
        /// </summary>
        public void UpdateMethod()
        {
            if (isVisible)
            {
                //calculate size of slider
                //size  = virtual height / window height.
                ResizeSlider();
                ChangeOffset();
                CheckFiles();
            }
        }

        /// <summary>
        /// Method used to keep the list of files displayed up-to-date
        /// </summary>
        public void CheckFiles()
        {
            //clear the list
            buttonList.Clear();

            //populate the list with the files in the directory
            string path = Directory.GetCurrentDirectory();

            //Dirsearch the path
            DirSearch(path, buttonList);
        }

        /*
         * function DirSearch
         * purpose: Iterate through the specified directory and add
         *  the files found to the listbox. 
         * input: 
         *  sDir - the name of the directory to be searched
         * output: none
         * properties modified: buttonList
         * 
         * NOTE: This method adapted from method suggesed by John T,
         *  on May 30th 2009,
         *  http://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c
         */
        /// <summary>
        /// Searches the current directory for files and adds them to the list as needed.
        /// Note: This method adapted from method suggested by John T,
        /// on May 30th, 2009,
        /// http://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c
        /// </summary>
        /// <param name="sDir">The current directory</param>
        /// <param name="buttonListIn">The list of buttons to be amended</param>
        static void DirSearch(string sDir, List<IBoxButton> buttonListIn)
        {
            try
            {

                foreach (string f in Directory.GetFiles(sDir))
                {
                    //Debug.Log(f);
                    //if the file is an xml, add to list
                    if (f.EndsWith(".xml")) //XML HARDCODED
                    {
                        //delete the path from f
                        string g = f.Remove(0, sDir.Length + 1);

                        //create a new button with f as text
                        IBoxButton newButton = new IBoxButton();
                        newButton.text = g;

                        //add newButton to list
                        buttonListIn.Add(newButton);
                    }
                }

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        /// <summary>
        /// Makes the slider size proportionate to the number of elements
        /// </summary>
        public void ResizeSlider()
        {
            //calculate size of slider
            //size  = virtual height / window height.
            float virtualHeight =
                ((buttonArrayCast.height + buttonArrayCast.spacing) * buttonList.Count);
            float windowHeight = height - (selectedField.height + selectedField.spacing);

            if (virtualHeight > windowHeight) //make sure slider isn't too big
            {
                vSlider.size = (int)(100 * windowHeight / virtualHeight);
            }

            else
            {
                vSlider.size = 50;
            }

            //Debug.Log("Height = " + windowHeight + "virtualH : " + virtualHeight);
            //Debug.Log(windowHeight / virtualHeight);
        }

        /// <summary>
        /// Makes the slider control work
        /// </summary>
        public void ChangeOffset()
        {
            //variables
            float virtualHeight =
                ((buttonArrayCast.height + buttonArrayCast.spacing) * buttonList.Count) + Y;
            sliderOffset = vSlider.value * virtualHeight / 100;

        }

        //place in Start()
        /// <summary>
        /// Method to initialize the control.
        /// When the control is first called, it sets the selected field text to "No item selected"
        /// Note: call this method in Start()
        /// </summary>
        public void InitMethod()
        {
            selectedField.text = "No item selected";
            CheckFiles();
        }
    }

    
    /// <summary>
    /// Event handler for buttonPanel
    /// Handles event raised when user clicks a button in the panel
    /// </summary>
    /// <param name="sender">reference to object which raised the event</param>
    /// <param name="e">Arguments</param>
    public delegate void ButtonPanelClickevent(object sender, EventArgs e);

    /// <summary>
    /// Defines a class which creates a simple panel of buttons
    /// </summary>
    [System.Serializable]
    public class ButtonPanel
    {
        /// <summary>
        /// Dimensions of the control's surroundings
        /// </summary>
        public int X, Y, xSpacing, ySpacing;

        /// <summary>
        /// Defines dimensions of the buttons in the array
        /// The text is not taken into account
        /// </summary>
        public IBoxButton buttonArrayCast;

        /// <summary>
        /// Array of buttons. The dimensions of the buttons are not considered.
        /// Only the text of each button is used to draw them
        /// </summary>
        public IBoxButton[] buttonArray;

        int outputIndex { get; set; }

        /// <summary>
        /// Event handler for event raised when button is clicked
        /// </summary>
        /// <returns>delegate?</returns>
        public ButtonPanelClickevent buttonPanelClickHandler;

        public void PanelButtonClicked(int buttonIndexIn, EventArgs e)
        {

            outputIndex = buttonIndexIn;
            
            buttonPanelClickHandler(this, e);
        }

        public void Draw()
        {
            //Draw the rectangle surrounding the panel
            GUI.Box(new Rect(X, Y, (2 * xSpacing) + buttonArrayCast.width,
                (ySpacing) + ((buttonArrayCast.height + buttonArrayCast.spacing) * buttonArray.Length)),
                "");

            //Draw all the buttons
            for (int i = 0; i < buttonArray.Length; i++)
            {
                if(
                GUI.Button(new Rect(X + buttonArrayCast.X, Y + buttonArrayCast.Y + (i * (buttonArrayCast.height + buttonArrayCast.spacing)),
                    buttonArrayCast.width, buttonArrayCast.height), buttonArray[i].text))
                    PanelButtonClicked(i, new EventArgs());
            }

        }

        public int getOutputIndex() { return outputIndex; }


    };

}
