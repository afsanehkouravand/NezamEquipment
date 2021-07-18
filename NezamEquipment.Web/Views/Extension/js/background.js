function checkForTargetMessageEditorUrl(tabId, changeInfo, tab) {
    // Check if the address contains the Message Editor URL;
    if (tab.url.indexOf(".codeproject.com/script/Forums/Edit.aspx?") > -1) {
        // ... show the page action.
        chrome.pageAction.show(tabId);
    }
    // Check if the address contains the Question URL;
    if (tab.url.indexOf(".codeproject.com/Questions/") > -1) {
        // ... show the page action.
        chrome.pageAction.show(tabId);
    }

};

// Add a handler to listen for Address Bar changes in the browser, set the callback function
chrome.tabs.onUpdated.addListener(checkForTargetMessageEditorUrl);