var myApp = angular.module('daysonlogapiApp', []);

myApp.service('dataService', function ($http) {
    this.getData = function () {
        var daysonlogapiUrl = 'http://dayson-log-api.gear.host/logs';
        console.log(daysonlogapiUrl);
        return $http({
            method: 'GET',
            url: daysonlogapiUrl
        });
    };

    this.getError = function () {
        var daysonlogapiUrl = 'http://dayson-log-api.gear.host/errors';
        console.log(daysonlogapiUrl);
        return $http({
            method: 'GET',
            url: daysonlogapiUrl
        });
    };
    
    this.clear = function () {
        var daysonlogapiUrl = 'http://dayson-log-api.gear.host/clear';
        console.log(daysonlogapiUrl);
        return $http({
            method: 'GET',
            url: daysonlogapiUrl
        });
    }
});

myApp.controller('daysonlogapiCtrl', function ($scope, dataService) {

    $scope.data = "";
    $scope.postcode = "";

    $scope.load = function () {
        dataService.getData().then(function (dataResponse) {
            $scope.data = dataResponse.data;
        });
    };

    $scope.error = function () {
        dataService.getError().then(function (dataResponse) {
            $scope.data = dataResponse.data;
        });
    };

    $scope.load();
});