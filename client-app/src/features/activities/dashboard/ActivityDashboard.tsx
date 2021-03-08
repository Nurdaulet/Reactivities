import { observer } from 'mobx-react-lite';
import React, { useEffect } from 'react';
import { Grid } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { useStore } from '../../../app/stores/store';
import ActivityFilters from './ActivityFilters';
import ActivityList from './ActivityList';



export default observer(function ActivityDashboard() {

    const { activityStore } = useStore();

    const { loadActivites, activityRegistry } = activityStore;

    useEffect(() => {
        if (activityRegistry.size === 0) loadActivites();
    }, [activityRegistry.size, loadActivites]);


    if (activityStore.initialLoading) return <LoadingComponent content={'Loading app'} />

    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList />
            </Grid.Column>
            <Grid.Column width='6'>
                <ActivityFilters />
            </Grid.Column>
        </Grid>
    )
})