angular.module('login-app').controller('resetPasswordController', [
 '$routeParams',
 function ($routeParams) {
   this.code = $routeParams.code;

   this.executeRequest = function () {
     this.requestSent = true;
   }
 }]);
