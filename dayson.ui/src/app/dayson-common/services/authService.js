angular.module('dayson-common').factory('authService', [
  '$http', '$q',
  function ($http, $q) {
    var authService = {};

    authService.login = function (email, password) {
      var deferred = $q.defer();

      deferred.reject();
      console.log('authService.login');

      return deferred.promise;
    }

    authService.registerCompany = function (companyName, email, password) {
      var deferred = $q.defer();

      deferred.reject();
      console.log('authService.registerCompany');

      return deferred.promise;
    }

    authService.requestPasswordReset = function (email) {
      var deferred = $q.defer();

      deferred.reject();
      console.log('authService.requestPasswordReset');

      return deferred.promise;
    }

    authService.resetPassword = function (code, password) {
      var deferred = $q.defer();

      deferred.reject();
      console.log('authService.resetPassword');

      return deferred.promise;
    }

    return authService;
  }
]);
