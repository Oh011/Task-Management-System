namespace Services.Abstractions.Common
{


    /// <summary>
    /// Represents high-level actions that can be performed on a project.
    /// </summary>
    public enum ProjectAction
    {


        UpdateProject,
        DeleteProject,
        ChangeProjectStatus,
        ManageMembers, //--> Add , searach for users to add
        // Add/remove users from project
        AssignProjectRoles,       // Assign roles within the project (Owner, Member, etc.)
        RemoveProjectMember,
        AddTask,



    }

    /// <summary>
    /// Represents actions that can be performed on individual project tasks.
    /// </summary>

    public enum ProjectTaskAction
    {

        // Task-level actions


        UpdateTask,
        DeleteTask,
        ChangeTaskStatus,
        AssignTask

    }

    public enum AuthorizationAction
    {

        ProjectTaskAction,
        ProjectAction


    }

}
