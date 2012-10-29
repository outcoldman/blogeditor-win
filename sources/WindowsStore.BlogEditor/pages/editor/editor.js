// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/editor/editor.html", {
        
        _fHtmlEditor: false,

        ready: function (element, options) {

            var that = this;

            document.getElementById("cmdSave")
                .addEventListener("click", function() {
                    that._doClickSave();
                }, false);
            
            document.getElementById("cmdDelete")
                .addEventListener("click", function () {
                    that._doClickDelete();
                }, false);
            
            document.getElementById("cmdSwitchToHtml")
                .addEventListener("click", function () {
                    that._doClickSwitch();
                }, false);
        },

        unload: function () {
            // TODO: Respond to navigations away from this page.
        },

        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            // TODO: Respond to changes in viewState.
        },
        
        _doClickSave : function() {
            // TODO: Save data
            this._goOutThisPage();
        },
        
        _doClickDelete: function () {
            var that = this;

            var msg = new Windows.UI.Popups.MessageDialog("Are you sure you want to delete current progress?");
            msg.commands.append(new Windows.UI.Popups.UICommand("Yes", function () {
                that._goOutThisPage();
            }));
            msg.commands.append(new Windows.UI.Popups.UICommand("No"));
            msg.defaultCommandIndex = 0;
            msg.cancelCommandIndex = 1;
            msg.showAsync();
        },
        
        _doClickSwitch: function () {
            var textHtmlEditor = document.getElementById("textHtmlEditor"); 
            var textRichEditor = document.getElementById("textRichEditor");

            if (this._fHtmlEditor) {
                textHtmlEditor.style.visibility = 'hidden';
                textRichEditor.style.visibility = 'visible';
                textRichEditor.innerHTML = window.toStaticHTML(textHtmlEditor.value);
            } else {
                textHtmlEditor.style.visibility = 'visible';
                textRichEditor.style.visibility = 'hidden';
                textHtmlEditor.value = textRichEditor.innerHTML;
            }
            this._fHtmlEditor = !this._fHtmlEditor;
        },
        
        _goOutThisPage: function() {
            if (WinJS.Navigation.canGoBack) {
                WinJS.Navigation.back();
            } else {
                WinJS.Navigation.navigate("/pages/home/home.html");
            }
        }
    });
})();
