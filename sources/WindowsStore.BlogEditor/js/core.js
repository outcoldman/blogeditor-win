// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

(function() {
    WinJS.Namespace.define("OutcoldSolutions", {
       newGuid : function() {
           var s4 = function () {
               return Math.floor(
                       Math.random() * 0x10000 /* 65536 */
                   ).toString(16);
           };

           return (
                   s4() + s4() + "-" +
                   s4() + "-" +
                   s4() + "-" +
                   s4() + "-" +
                   s4() + s4() + s4()
               );
       } 
    });
})();