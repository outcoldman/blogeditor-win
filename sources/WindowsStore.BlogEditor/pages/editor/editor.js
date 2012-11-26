// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/editor/editor.html", {
        
        _fHtmlEditor: false,
        _keyboardFix: null,
        _textHtmlEditor: null,
        _textRichEditor: null,
        _articleTitle : null,

        ready: function (element, options) {

            var that = this;

            this._textHtmlEditor = document.getElementById("textHtmlEditor");
            this._textRichEditor = document.getElementById("textRichEditor");
            this._articleTitle = document.getElementById("articleTitle");

            document.getElementById("cmdPublish")
                .addEventListener("click", function() {
                    that._doClickPublish();
                }, false);
            
            document.getElementById("cmdClear")
                .addEventListener("click", function () {
                    that._doClickClear();
                }, false);
            
            document.getElementById("cmdSwitchToHtml")
                .addEventListener("click", function () {
                    that._doClickSwitch();
                }, false);
            
            // Special fix for case when Windows shows keyboard
            this._keyboardFix = document.getElementById("keyboardFix");
            var inputPane = Windows.UI.ViewManagement.InputPane.getForCurrentView();
            inputPane.addEventListener("hiding", function (eventArgs) {
                    eventArgs.ensuredFocusedElementInView = true;
                    that._keyboardFix.style.height = eventArgs.occludedRect.height + "px";
                }, false);
            inputPane.addEventListener("showing", function (eventArgs) {
                    eventArgs.ensuredFocusedElementInView = true;
                    that._keyboardFix.style.height = eventArgs.occludedRect.height + "px";
                }, false);
        },

        _doClickPublish : function() {
            var that = this;

            var publisher = new OutcoldSolutions.BlogEditor.MetaWeblogPublisher();
            var articleBody = null;
            if (this._fHtmlEditor) {
                articleBody = this._textHtmlEditor.value;
            } else {
                articleBody = this._textRichEditor.innerHTML;
            }

            publisher.publishAsync(this._articleTitle.innerHTML, articleBody).done(function (a) {
                that._clearContent();
            });
        },
        
        _doClickClear: function () {
            var that = this;

            var msg = new Windows.UI.Popups.MessageDialog("Are you sure you want to clear all text?");
            msg.commands.append(new Windows.UI.Popups.UICommand("Yes", function () {
                that._clearContent();
            }));
            msg.commands.append(new Windows.UI.Popups.UICommand("No"));
            msg.defaultCommandIndex = 0;
            msg.cancelCommandIndex = 1;
            msg.showAsync();
        },
        
        _doClickSwitch: function () {
            this._fHtmlEditor = !this._fHtmlEditor;
            if (this._fHtmlEditor) {
                this._showHtmlEditor();
                this._textHtmlEditor.value = this._textRichEditor.innerHTML;
            } else {
                this._showRichEditor();
                this._textRichEditor.innerHTML = window.toStaticHTML(this._textHtmlEditor.value);
            }
        },
        
        _clearContent: function() {
            this._articleTitle.innerHTML = "Blog post title";
            this._textRichEditor.innerHTML = "<p>Content goes here.</p>";

            this._fHtmlEditor = false;
            this._showRichEditor();
        },
        
        _showHtmlEditor : function() {
            this._textHtmlEditor.style.visibility = 'visible';
            this._textRichEditor.style.visibility = 'hidden';
        },
        
        _showRichEditor : function() {
            this._textHtmlEditor.style.visibility = 'hidden';
            this._textRichEditor.style.visibility = 'visible';
        }
    });
})();
