chrome.browserAction.onClicked.addListener(function(tab) {
  //chrome.tabs.insertCSS(tab.id, { file: "bundle.css" });
  chrome.tabs.executeScript(tab.id, { file: "bundle.js" });
});