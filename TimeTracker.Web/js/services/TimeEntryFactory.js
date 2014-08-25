'use strict';

timeTrackerApp.factory('TimeEntryFactory', function ($http, $q) {
    var baseUrl = '/api/entry';
    var TimeEntryFactory = {};
    
    TimeEntryFactory.getGridColumns = function (startDate) {
        var deferred = $q.defer();
        
        $http({ method: 'GET', url: baseUrl + '/GetGridColumns?startDate=' + startDate })
            .success(function(data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });
        
        return deferred.promise;
    };

    TimeEntryFactory.getGridData = function (weekStartDate, weekEndDate) {
        var deferred = $q.defer();
        
        $http({ method: 'GET', url: baseUrl + '/?weekStartDate=' + weekStartDate + '&weekEndDate=' + weekEndDate })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });
        
        return deferred.promise;
    };

    TimeEntryFactory.getTaskLookup = function () {
        var deferred = $q.defer();

        $http({ method: 'GET', url: baseUrl + '/GetTaskList'})
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });
        
        return deferred.promise;
    };
    
    TimeEntryFactory.addTask = function (task, weekStartDate) {
        var deferred = $q.defer();
        var header = {
            WeekStartDate: weekStartDate,
            Task: {
                Name: task
            }
        };
        
        $http({ method: 'POST', url: baseUrl + '/CreateEntryHeader', data: header })
            .success(function (data, status, headers, config) {
                deferred.resolve(data, status);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });
        
        return deferred.promise;
    };

    TimeEntryFactory.deleteTask = function (headerId) {
        var deferred = $q.defer();

        $http({ method: 'POST', url: baseUrl + '/DeleteEntryHeader', data: headerId })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });

        return deferred.promise;
    };

    TimeEntryFactory.createEntry = function (entry) {
        var deferred = $q.defer();

        $http({ method: 'POST', url: baseUrl + '/CreateEntry', data: entry })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });

        return deferred.promise;
    };

    TimeEntryFactory.updateEntry = function (entry) {
        var deferred = $q.defer();

        $http({ method: 'POST', url: baseUrl + '/UpdateEntry', data: entry })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(status);
            });
        
        return deferred.promise;
    };

    return TimeEntryFactory;
});
