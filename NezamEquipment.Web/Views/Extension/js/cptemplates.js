// CodeProject Template Items Chrome Extension
// Author: David Auld (c) 2011
// Version 2.0

// For more write up see: http://www.codeproject.com/Articles/244083/Google-Chrome-Extension-CodeProject-Template-Items

// ----------------------------------------------------------------------
// Revision Changes
// ----------------------------------------------------------------------
//
//  Version 2.0 (29th August 2014)
//  FIX: migrated to Google Chrome Extensions Manfist V2 requirements. No new functionality.
//  UPG: Upgraded jQuery to v1.11.1
//  ADD: Added basic Google Analytics tracking - This is to know if people are using and whether to continue supporting or kill off.
//
//  Version 1.1 (16th September 2011)
//  FIX: initStorage element name fixed
//  FIX: clear form on ADD
//  FIX: Managed to break V1 on release (don't know what I did though!)
//  ADD: Additional Template Type Snippet Added  
//  ADD: Editor Type Filtering
//  ADD: Selector Type Filtering
// ----------------------------------------------------------------------

//Local variables
var templateStore = new Array();

var templateSelectMode = false;       //The selector page will set this flag true; editor - false
var templateEditMode = false;         //True = Edit Mode; false = default Add mode
var templateEditModeItem = 0;         //The index of item in the template Store being edited.

//Template Object Constructor
function Template() {
    this.Type = "";
    this.Title = "";
    this.Subject = "";
    this.Body = "";
}

//initialise the local storage
function initStorage() {
    if (localStorage["TemplateStore"] == undefined) {
        localStorage["TemplateStore"] = "{}";
    }
    else {
        templateStore = JSON.parse(localStorage["TemplateStore"]);
    }
}

function initEditor() {
    //Set the editor/selector flag
    templateSelectMode = false;

    //Setup button JQueryUI
    $("#buttonAdd").button();
    $("#buttonEdit").button();
    $("#buttonClone").button();
    $("#buttonDelete").button();
    $("#buttonCancel").button();
    $("#buttonSave").button();

    //Set up button handlers
    $("#buttonAdd").click(buttonAdd_Click);
    $("#buttonEdit").click(buttonEdit_Click);
    $("#buttonClone").click(buttonClone_Click);
    $("#buttonDelete").click(buttonDelete_Click);
    $("#buttonCancel").click(buttonCancel_Click);
    $("#buttonSave").click(buttonSave_Click);

    $("#selectFilter").change(selectFilter_Change);
    $("#selectTemplates").click(selectTemplates_Click);
    
    //Default radio values
    $("#RadioTypeMessage").prop("checked", true);
    $("#RadioTypeAnswer").prop("checked", false);
    $("#RadioTypeSnippet").prop("checked", false);              //v1.1 added
    
    //Initialise Local Storage
    initStorage();
    
    //Setup Editor view
    initEditorView();
}

function initEditorView() {
    //Setup the editor display
    $("#templateLoading").show();
    $("#templateListing").hide();
    $("#templateEdit").hide();
    $("#buttonAdd").show();
    $("#buttonEdit").hide();
    $("#buttonClone").hide();
    $("#buttonDelete").hide();

    //Load the templates from the local store
    loadTemplates();
}

function initSelector() {
    //Set the editor/selector flag
    templateSelectMode = true;

    //Setup button JQueryUI
    $("#buttonSelect").button();
    
    //Set up button handlers
    $("#buttonSelect").click(buttonSelect_Click);
    $("#selectTemplates").click(selectTemplates_Click);
    $("#selectFilter").change(selectFilter_Change);

    //Initialise Local Storage
    initStorage();

    //Setup Editor view
    initSelectorView();
}

//Load the templates from the local store
function initSelectorView() {
    setTimeout(loadTemplates());
}

//-----------------------------------------
//     Button Events - Editor
//-----------------------------------------
function buttonAdd_Click() {
    templateEditMode = false;
    $("#templateLoading").html("Add new item to template store;");
    $("#templateListing").hide();

    //Clear the fields
    $("#TextTitle").val("");
    $("#TextSubject").val("");
    $("#TextBody").val("");

    //Set default type based on filter
    switch ($("#selectFilter option:selected").val()){
        case "Message":
            $("#RadioTypeMessage").prop('checked', true);
            break;

        case "Answer":
            $("#RadioTypeAnswer").prop('checked', true);
            break;

        case "Snippet":
            $("#RadioTypeSnippet").prop('checked', true);
            break;

        default:
            $("#RadioTypeMessage").prop('checked', true);
            break;
    }

    $("#templateEdit").show();
}

function buttonEdit_Click() {
    templateEditMode = true;
    templateEditModeItem = parseInt($("#selectTemplates option:selected").val());

    //V1.1 Change Switch block replaces If and added Snippet handling
    switch (templateStore[templateEditModeItem].Type) {
        case "Message":
            $("#RadioTypeMessage").prop('checked', true);
            break;

        case "Answer":
            $("#RadioTypeAnswer").prop('checked', true);
            break;

        case "Snippet":
            $("#RadioTypeSnippet").prop('checked', true);
            break;
    }

    $("#TextTitle").val(templateStore[templateEditModeItem].Title);
    $("#TextSubject").val(templateStore[templateEditModeItem].Subject);
    $("#TextBody").val(templateStore[templateEditModeItem].Body);

    $("#templateListing").hide();
    $("#templateEdit").show();
}

function buttonClone_Click() {
    //Get the current selected item
    var original = templateStore[parseInt($("#selectTemplates option:selected").val())];
    
    //Perfrom a deep copy of the original using JQuery
    var copy = $.extend(true, {}, original);

    //Push it into the store at the end
    templateStore.push(copy);

    //save
    saveToLocalStorage();

    initEditorView();
}

function buttonDelete_Click() {
    //Which item selected
    var item = $("#selectTemplates option:selected").val();

    var response = confirm("Delete item: " + $("#selectTemplates option:selected").text());

    if (response) {
        //Delete the selected item
        templateStore.splice(parseInt(item), 1);
    }

    //Write changes to local storage
    saveToLocalStorage();

    initEditorView();
}

function buttonCancel_Click() {
    //Return reload templates and show listing
    initEditorView();
    loadTemplates();
}

function buttonSave_Click() {
    //Create new item
    var item = new Template();

    //Add item properties
    item.Type = $("input[name=RadioType]:checked").val();
    item.Title = $("#TextTitle").val().toString();
    item.Subject = $("#TextSubject").val().toString();
    item.Body = $("#TextBody").val().toString();

    if (templateEditMode) {
        //Edit Mode
        templateStore[templateEditModeItem] = item;
        templateEditMode = false;
    }
    else {
        //Add Mode
        templateStore.push(item);
    }

    //Save to local storage
    saveToLocalStorage();

    //Restart
    initEditorView();
}

//-----------------------------------------
//     Button Events - Selector
//-----------------------------------------
function buttonSelect_Click() {
    //
    //Get the current window and then the current tab of that window
    //This should relate to the page on which to do the injection
    chrome.windows.getCurrent(function (theWindow) {
        chrome.tabs.getSelected(theWindow.id, function (theTab) { 
            injectToTab(theTab) });
    });
}

function injectToTab(tab) {
    //Build Valid Content for remote execution
    var templateItem = parseInt($("#selectTemplates option:selected").val());
    var subject = templateStore[templateItem].Subject;
    var body = templateStore[templateItem].Body;
    
    // Check if the address contains the Message Editor URL;
    if (tab.url.indexOf(".codeproject.com/script/Forums/Edit.aspx?") > -1) {
        
        //Append the subject
        chrome.tabs.executeScript(tab.id, { code:
            "document.getElementById('ctl00_MC_Subject').value += unescape('" + escape(subject) + "');"
        });
        
        //Append the body
        chrome.tabs.executeScript(tab.id, { code:
            "document.getElementById('ctl00_MC_ContentText_MessageText').value += unescape('" + escape(body) + "');"
        });
    }

    // Check if the address contains the Question URL;
    if (tab.url.indexOf(".codeproject.com/Questions/") > -1) {
        //Append the answer
        chrome.tabs.executeScript(tab.id, { code:
            "document.getElementById('ctl00_ctl00_MC_AMC_PostEntryObj_Content_MessageText').value += unescape('" + escape(body) + "');"
        });
    }
}

//-----------------------------------------------
// Other Events
//-----------------------------------------------
function selectTemplates_Click() {
    //Which item selected
    var item = $("#selectTemplates option:selected").val();

    if (!item || parseInt(item) < 0) {
        //No item selected
        //Don't know if i need this yet, so left as a placeholder :-)
    }
    else {
        if (!templateSelectMode) {
            $("#buttonEdit").show();
            $("#buttonClone").show();
            $("#buttonDelete").show();
        }
    }
}

//V1.1
function selectFilter_Change() {
    //Filter Mode Change
    loadTemplates();
}

//-----------------------------------------------
// Misc Functions
//-----------------------------------------------

function saveToLocalStorage() {
    //Write out the template store to the local storage
    localStorage["TemplateStore"] = JSON.stringify(templateStore);
}

function loadTemplates() {
    chrome.windows.getCurrent(function (theWindow) {
        chrome.tabs.getSelected(theWindow.id, function (theTab) {
            // Check if the address contains the Message URL;
            if (theTab.url.indexOf(".codeproject.com/script/Forums/Edit.aspx?") > -1) {
                loadTemplatesPart2("Message");
            }
            else {
                // Check if the address contains the Question URL;
                if (theTab.url.indexOf(".codeproject.com/Questions/") > -1) {
                    loadTemplatesPart2("Answer");
                }
                else {
                    loadTemplatesPart2("");
                }
            }
        });
    });
}

function loadTemplatesPart2(urlType) {
    //Clear the list box of any items
    $("#selectTemplates").html(""); //Clear the existing selection box

    if (templateStore.length > 0) {
        //Load Template Items
        $("#templateLoading").html("Template store contains: " + templateStore.length + " item(s).");
        
        //get the filter mode
        var filter = $("#selectFilter option:selected").val();

        //Get each items Title and add it to the list subject to filters
        for (item in templateStore) {
            var addit = false;
            switch (filter) {
                case "All":
                    //Add all items
                    addit = true;
                    break;

                case "Auto":
                    //Always add the snippets regardless of Answer/Message
                    if (templateStore[item].Type == "Snippet") {
                        addit = true;
                    }
                    else {
                        //if the item matches the page (message/answer) then addit
                        addit = (templateStore[item].Type == urlType);
                    }
                    break;

                default:
                    //Add if item == filter type
                    addit = (templateStore[item].Type == filter); 
            }

            if (addit) {
                $("#selectTemplates").html($("#selectTemplates").html() + "<option value=\"" + item.toString() + "\">" + templateStore[item].Title + "</option>");
            }
        }
        $("#templateLoading").html($("#templateLoading").html() + " Displaying: " + $("#selectTemplates").prop("length").toString() + " item(s)");
        $("#templateListing").show();
    }
    else {
        // No items to Load
        $("#templateLoading").html("No items located in local store.");
        $("#templateListing").show();

        if (!templateSelectMode) {
            $("#templateEdit").hide();
            $("#buttonEdit").hide();
            $("#buttonClone").hide();
            $("#buttonDelete").hide();
        }
        else {
            $("#buttonSelect").hide();
        }
    }
    
    //Add always is shown (in editor form).
    if (!templateSelectMode) {
        $("#buttonAdd").show(); 
    }

}

function addTemplateToStore(objectType, objectTitle, objectSubject, objectBody) {

    var item = new Template();
    item.Type = objectType.toString();
    item.Title = objectSubject.toString();
    item.Subject = objectSubject.toString();
    item.Body = objectBody.toString();

    //Add the new element to the store
    templateStore.push(objTemplate);
}