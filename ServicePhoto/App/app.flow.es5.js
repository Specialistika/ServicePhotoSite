'use strict';

(function () {
    angular.module('app').config(configureFlow);

    configureFlow.$inject = ['flowFactoryProvider'];

    function configureFlow(flowFactoryProvider) {
        flowFactoryProvider.defaults = {
            permanentErrors: [404, 500, 501],
            maxChunkRetries: 1,
            chunkRetryInterval: 5000,
            simultaneousUploads: 4
        };

        flowFactoryProvider.on('catchAll', function (event) {
            console.log('catchAll', arguments);
            //for (var a in FlowFile.file.name){
            //    var b = a.toString();
            //}

            //FlowFile.file.name.forEach(function (entry) {
            //    console.log(entry);
            //});
            //if (event === "complete") {
            //    var n = Flow.FlowFile.name;
            //    var c = Flow.FlowFile.file;
            //}
        });

        // Can be used with different implementations of Flow.js
        flowFactoryProvider.factory = fustyFlowFactory;
    }
})();

