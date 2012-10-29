(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            document.getElementById("btnAddBlogPost").addEventListener("click", function() {
                WinJS.Navigation.navigate("/pages/editor/editor.html");
            }, false);
        }
    });
})();
