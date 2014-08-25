'use strict';

timeTrackerApp.controller('LoginController', ['$scope', '$location', '$window', 'AuthenticationService', function ($scope, $location, $window, AuthenticationService) {
    $scope.account = {};
    $scope.account.username = null;
    $scope.account.password = null;
    $scope.hasError = false;
    $scope.status = "";

    $scope.login = function () {
        AuthenticationService.login($scope.account)
            .then(function (data) {
                if (data == "true") {
                    $location.path("/");
                    $scope.hasError = false;
                    $scope.status = "";
                } else {
                    $scope.status = "Login failed. Invalid username or password.";
                    $scope.hasError = true;
                }
            });
    };
    
}]);