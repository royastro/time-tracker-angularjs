'use strict';

var TimeEntryController = function ($scope, $modalInstance, selectedEntry) {
    $scope.selectedEntry = selectedEntry;

    $scope.save = function () {
        $modalInstance.close($scope.selectedEntry);
    };
    
    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
    };
};