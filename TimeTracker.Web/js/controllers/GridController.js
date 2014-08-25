'use strict';

timeTrackerApp.controller('GridController', ['$scope', '$timeout', '$modal', 'TimeEntryFactory', '$log', 'AuthenticationService', function ($scope, $timeout, $modal, TimeEntryFactory, $log, AuthenticationService) {
    $scope.status;
    $scope.gridColumns;
    $scope.gridData;
    $scope.dt;
    $scope.taskLookup = {};
    $scope.selectedTask = undefined;
    $scope.selectedEntry = {};
    $scope.hasError = false;
    $scope.isLoggedIn = false;
    init();

    function init() {
        $scope.dt = new Date();
        getGridColumns();
        TimeEntryFactory.getTaskLookup().then(function(data) {
                $scope.taskLookup = data;
        });
        CheckLoginStatus();
    }    

    function getGridColumns() {
        TimeEntryFactory.getGridColumns(getStartOrEndOfWeek(true, $scope.dt))
            .then(function (data) {
                $scope.gridColumns = data;
                getGridData();
            });
    }
    
    function getGridData() {
        TimeEntryFactory.getGridData(getStartOrEndOfWeek(true, $scope.dt), getStartOrEndOfWeek(false, $scope.dt))
            .then(function(data) {
                $scope.gridData = data;
                $scope.hasError = false;
            },
            function(data) {
                $scope.status = data;
                $scope.hasError = true;
            });
    }
    
    function getStartOrEndOfWeek(isStart, date)
    {
        if (isStart) {
            return moment(date).startOf('isoweek').format('YYYY-MM-DD');
        } else {
            return moment(date).endOf('isoweek').format('YYYY-MM-DD');
        }
    }
    
    function CheckLoginStatus() {
        $scope.isLoggedIn = AuthenticationService.isLoggedIn();
    };

    // Start - Task Methods
    $scope.addTask = function () {
        TimeEntryFactory.addTask($scope.selectedTask, getStartOrEndOfWeek(true, $scope.dt))
            .then(function(data) {
                if (data == "") {
                    getGridData();
                    $scope.hasError = false;
                } else {
                    $scope.hasError = true;
                    $scope.status = data;
                }
            },
            function (error) {
                $scope.hasError = true;
                $scope.status = error;
            });
    };

    $scope.deleteTask = function (entryHeaderId) {
        TimeEntryFactory.deleteTask(entryHeaderId)
            .then(function(data) {
                getGridData();
                $scope.hasError = false;
            },
            function(data) {
                $scope.hasError = true;
                $scope.status = data;
            });
    };
    // End - Task Methods

    // Start - Calendar Methods
    $scope.onDateChanged = function() {
        getGridColumns();
    };
    
    $scope.showWeeks = true;
    $scope.toggleWeeks = function () {
        $scope.showWeeks = !$scope.showWeeks;
    };

    $scope.clear = function () {
        $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.toggleMin = function () {
        $scope.minDate = ($scope.minDate) ? null : new Date();
    };
    $scope.toggleMin();

    $scope.open = function () {
        $timeout(function () {
            $scope.opened = true;
        });
    };

    $scope.dateOptions = {
        'year-format': "'yy'",
        'starting-day': 1
    };
    // End - Calendar Methods

    
    $scope.showTimeEntryDialog = function() {
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: TimeEntryController,
            resolve: {
                selectedEntry: function() {
                    return $scope.selectedEntry;
                }
            }
        });
        
        modalInstance.result.then(function (result) {
            $log.info(result);
            saveEntry(result);
        }, function () {
            
        });
    };
    
    $scope.editEntry = function (headerId, entryId, entryDate, hours, notes) {
        $scope.selectedEntry = {
            Id: entryId,
            EntryHeader_Id: headerId,
            EntryDate: entryDate,
            Hours: hours,
            Notes: notes
        };
        $scope.showTimeEntryDialog();
    };
    
    function saveEntry(result) {
        if (result.Id == 0) {
            TimeEntryFactory.createEntry(result)
                .then(function(data) {
                    getGridData();
                    $scope.hasError = false;
                },
                function (data) {
                    $scope.status = data;
                    $scope.hasError = true;
                });
        } else {
            TimeEntryFactory.updateEntry(result)
                .then(function (data) {
                    getGridData();
                    $scope.hasError = false;
                },
                function (data) {
                    $scope.status = data;
                    $scope.hasError = true;
                });
        }
    }

}]);