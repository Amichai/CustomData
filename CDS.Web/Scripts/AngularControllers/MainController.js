function TodoCtrl($scope, $http) {
    $scope.home_active = "active";
    $scope.create_active = "";
    $scope.data_active = "";
    $http.get(urlRoot + 'api/taskapi/getTasks').success(function (result) {
        $scope.Tasks = result;
    });
    $scope.submitTask = function () {
        $http.post(urlRoot + 'api/taskapi/postTask', $scope.inspectionTask).success(function () {
        });
        resetTabs();
        $scope.home_active = "active";
    };
    $scope.inspectionTask;
    $scope.taskClick = function (task) {
        task.HitCount++;
        task.ProgressVal = Math.min(task.HitCount * 100.0 / task.CompletedAfter, 100);
        $http.post(urlRoot + 'api/taskapi/TaskClick', task).success(function () {
        });
    };
    $scope.deleteTask = function (task) {
        for(var i = $scope.Tasks.length - 1; i >= 0; i--) {
            if($scope.Tasks[i].ID == task.ID) {
                $scope.Tasks.splice(i, 1);
            }
        }
        $http.post(urlRoot + 'api/taskapi/TaskDelete', task).success(function () {
        });
    };
    $scope.edit = function (task) {
        $scope.inspectionTask = task;
        resetTabs();
        $scope.create_active = "active";
    };
    function resetTabs() {
        $scope.home_active = "";
        $scope.create_active = "";
        $scope.data_active = "";
    }
    $scope.home = function () {
        resetTabs();
        $scope.home_active = "active";
    };
    $scope.create = function () {
        resetTabs();
        $scope.create_active = "active";
    };
    $scope.data = function () {
        resetTabs();
        $scope.data_active = "active";
    };
}
