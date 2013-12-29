function TaskCtrl($scope, $http) {
    $scope.home_active = "active";
    $scope.create_active = "";
    $scope.data_active = "";

    //$http.get(urlRoot + 'api/taskapi/ConnectionTest').success(function (result) {
    //    console.log("Connection test: " + result);
    //});

    $http.get(urlRoot + 'api/taskapi/getTasks').success(function (result) {
        $scope.Tasks = result;
    });
    $scope.submitTask = function () {
        $http.post(urlRoot + 'api/taskapi/postTask', $scope.inspectionTask).success(function (tasks) {
            $scope.Tasks = tasks;
        });
        resetTabs();
        $scope.home_active = "active";
    };
    $scope.inspectionTask;
    function resetInspectionTask() {
        $scope.inspectionTask = new Object();
        $scope.inspectionTask.Name = '';
        $scope.inspectionTask.Description = '';
        $scope.inspectionTask.CompletedAfter = 100;
        $scope.inspectionTask.HitCount = 0;
    }
    resetInspectionTask();
    $scope.taskClick = function (task) {
        task.HitCount++;
        task.ProgressVal = Math.min(task.HitCount * 100.0 / task.CompletedAfter, 100);
        $http.post(urlRoot + 'api/taskapi/TaskClick', task).success(function () {
        });
    };
    $scope.deleteTask = function (task) {
        for (var i = $scope.Tasks.length - 1; i >= 0; i--) {
            if ($scope.Tasks[i].ID == task.ID) {
                $scope.Tasks.splice(i, 1);
            }
        }
        $http.post(urlRoot + 'api/taskapi/TaskDelete', task).success(function () {
        });
    };
    function swap(array_object, index_a, index_b) {
        var temp = array_object[index_a];
        array_object[index_a] = array_object[index_b];
        array_object[index_b] = temp;
    }
    $scope.moveDown = function (task) {
        var swapIndex = -1;
        for (var i = 0; i < $scope.Tasks.length - 1; i++) {
            if ($scope.Tasks[i].ID == task.ID) {
                swapIndex = i;
                break;
            }
        }
        if (swapIndex == -1) {
            return;
        }
        swap($scope.Tasks, swapIndex, swapIndex + 1);
    };
    $scope.moveUp = function (task) {
        var swapIndex = -1;
        for (var i = 1; i < $scope.Tasks.length; i++) {
            if ($scope.Tasks[i].ID == task.ID) {
                swapIndex = i;
                break;
            }
        }
        console.log(i);
        if (swapIndex == -1) {
            return;
        }
        swap($scope.Tasks, swapIndex, swapIndex - 1);
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
        resetInspectionTask();
    };
    $scope.create = function () {
        resetTabs();
        $scope.create_active = "active";
    };
    $scope.data = function () {
        resetTabs();
        $scope.data_active = "active";
    };
    $scope.toggleSelected = function (task) {
        if (task.selected == 'undefined') {
            task.selected = true;
        } else {
            task.selected = !task.selected;
        }
    };
    $scope.dateMin;
    $scope.dateMax;

    $scope.plot = function () {
        var indicies = $.find('.selected > input').map(function (n) { return n.value; });
        if (indicies.length == 0) {
            return;
        }
        var url = urlRoot + 'api/taskapi/getHits';
        $http.post(url, indicies).success(function (result) {
            chart2(result);
        });
    }
}
