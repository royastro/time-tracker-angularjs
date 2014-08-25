'use strict';

timeTrackerApp.directive('combobox', function() {
    return {
        restrict: 'E',
        transclude: true,
        template: '<select class="selectpicker" ng-transclude></select>',
        replace: false,
        compile: function (elem, attrs, transcludeFn) {
            return function(scope, element, attrs) {
                $(element).selectpicker();
            };
            
        }
    };

});