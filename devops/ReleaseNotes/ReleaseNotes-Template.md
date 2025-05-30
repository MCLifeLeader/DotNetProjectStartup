# Release Notes
-----
<div style="margin-bottom: 0.75rem;    border-style: solid;
    border-width: 0.0625rem;
    border-left-width: 0.375rem;
    display: inline-block;
    font-weight: 600;
    padding: 2px 0.5625rem;border-color: #214c62;">
   <div>
      <strong>Build:</strong> {{buildDetails.buildNumber}}
   </div>
   <div>
      <strong>Branch:</strong> {{buildDetails.sourceBranch}}
   </div>
   <div>
      <strong>Release Description:</strong> {{releaseDetails.description}}
   </div>
</div>

# Summary
{{#forEach this.workItems}}
{{#if isFirst}}## WorkItems {{/if}}
{{#if (contains (lookup this.fields 'System.WorkItemType') 'Bug')}}
*  [{{this.id}}](https://dev.azure.com/agameempowerment/The_Citizen_Developer/_workitems/edit/{{this.id}})  {{lookup this.fields 'System.Title'}} <span style="color:red">(Bug)</span> [{{lookup this.fields 'System.Tags'}}]
{{/if}}
{{#if (contains (lookup this.fields 'System.WorkItemType') 'User Story')}}
*  [{{this.id}}](https://dev.azure.com/agameempowerment/The_Citizen_Developer/_workitems/edit/{{this.id}})  {{lookup this.fields 'System.Title'}} <span style="color:blue">(User Story)</span> [{{lookup this.fields 'System.Tags'}}]
{{/if}}
{{/forEach}}

# List of work items released with details
{{#forEach this.workItems}}
{{#if isFirst}}## WorkItems {{/if}}
{{#if (contains (lookup this.fields 'System.WorkItemType') 'Bug')}}
*  [{{this.id}}](https://dev.azure.com/agameempowerment/The_Citizen_Developer/_workitems/edit/{{this.id}})  {{lookup this.fields 'System.Title'}}
   - **WIT** {{lookup this.fields 'System.WorkItemType'}}
   - **Tags** {{lookup this.fields 'System.Tags'}}
   - **Description** {{{lookup this.fields 'System.Description'}}}
   - **Release Note Title** {{{lookup this.fields 'Custom.ReleaseNoteTitle'}}}
   - **Release Note Description** {{{lookup this.fields 'System.ReleaseNoteDescription'}}}
{{/if}}
{{#if (contains (lookup this.fields 'System.WorkItemType') 'User Story')}}
*  [{{this.id}}](https://dev.azure.com/agameempowerment/The_Citizen_Developer/_workitems/edit/{{this.id}})  {{lookup this.fields 'System.Title'}}
   - **WIT** {{lookup this.fields 'System.WorkItemType'}}
   - **Tags** {{lookup this.fields 'System.Tags'}}
   - **Description** {{{lookup this.fields 'System.Description'}}}
   - **Release Note Title** {{{lookup this.fields 'Custom.ReleaseNoteTitle'}}}
   - **Release Note Description** {{{lookup this.fields 'System.ReleaseNoteDescription'}}}
{{/if}}
{{/forEach}}