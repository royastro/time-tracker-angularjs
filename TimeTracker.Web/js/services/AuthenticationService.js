'use strict';

timeTrackerApp.factory('AuthenticationService', function ($http, $q) {
    var baseUrl = '/api/security';
    var isLoggedIn = false;
    var user = {
        username: null
    };

    var AuthenticationService = {};

    AuthenticationService.getCurrentUser = function () {
        return $http.get(baseUrl + '/GetCurrentUser');
    };

    AuthenticationService.init = function () {
        var deferred = $q.defer();

        $http({ method: 'GET', url: baseUrl + '/IsLoggedIn' })
            .success(function (data, status, headers, config) {
                isLoggedIn = (data == "true");
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });

        return deferred.promise;
    };

    AuthenticationService.login = function (login) {
        var deferred = $q.defer();

        $http({ method: 'POST', url: baseUrl + '/login', data: login })
            .success(function (data, status, headers, config) {
                user.username = login.username;
                isLoggedIn = (data == "true");
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });

        return deferred.promise;
    };

    AuthenticationService.logout = function() {
        var deferred = $q.defer();
        $http({ method: 'POST', url: baseUrl + '/LogOff' })
            .success(function (data, status, headers, config) {
                isLoggedIn = false;
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });

        return deferred.promise;
    };

    AuthenticationService.isLoggedIn = function() {
        return isLoggedIn;
    };

    return AuthenticationService;
});