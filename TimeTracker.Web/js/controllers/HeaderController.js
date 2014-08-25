'use strict';

timeTrackerApp.controller('HeaderController', function ($scope, $log, $location, AuthenticationService) {
    $scope.user = {
        username: null
    };
    
    init();
    
    function init() {
        AuthenticationService.getCurrentUser().success(function(data) {
            $scope.user.username = data.UserName;
        });
    }

    $scope.preferences = function() {
    };

    $scope.logout = function logout() {
        AuthenticationService.logout().then(function(data) {
            $location.path("/login");
        });
    };

});