angular.module('login-app').controller('registerController', [
 function () {
   this.executeRegister = function () {
     this.submitted = true;
     this.errors = {};
     this.errors.companyAlreadyExists = true;
     this.errors.emailAlreadyExists = true;
     this.requestSent = true;
   }
 }]);
