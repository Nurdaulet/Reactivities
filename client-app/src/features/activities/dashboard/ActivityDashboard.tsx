import React from 'react';
import { Grid, List } from 'semantic-ui-react';
import { Activity } from '../../../app/models/activity';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../form/ActivityForm';
import ActivityList from './ActivityList';

interface Props {
    activities: Activity[];
    selectedActivity: Activity | undefined;
    selectActivity: (id: string) => void;
    cancelSelectActivity: () => void;
    editMode: boolean;
    handleFormOpen: (id: string) => void;
    handleFormClose: () => void;
    createOrEditActivity: (activity: Activity) => void;
    deleteActivity: (id: string) => void;
    submitting: boolean;
}

export default function ActivityDashboard({ activities, selectedActivity, selectActivity, cancelSelectActivity, handleFormClose, handleFormOpen, editMode, createOrEditActivity, deleteActivity, submitting }: Props) {
    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList activities={activities} selectActivity={selectActivity} deleteActivity={deleteActivity} submitting={submitting}/>
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity && !editMode && <ActivityDetails activity={selectedActivity} cancelSelectActivity={cancelSelectActivity} handleFormOpen={handleFormOpen} />}
                {editMode && <ActivityForm handleFormClose={handleFormClose} activity={selectedActivity} createOrEditActivity={createOrEditActivity} submitting={submitting}  />}
            </Grid.Column>
        </Grid>
    )
}   