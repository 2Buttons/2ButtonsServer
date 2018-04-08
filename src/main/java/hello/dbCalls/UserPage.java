package hello.dbCalls;

import java.sql.*;

public class UserPage {

    public static ResultSet getUserInfo(Connection con, int userID, int getUserID) {
        ResultSet rs = null;
        PreparedStatement st;

        try {
            st = con.prepareStatement("select * from dbo.getUserInfo(?, ?)");

            st.setInt(1, userID);
            st.setInt(2, getUserID);

            rs = st.executeQuery();

            if (userID != getUserID) {
                //if (rs.next()) {
                    if (rs.getInt("youFollowed") == 1) {
                        updateVisits(con, userID, getUserID);
                    }
                //}
            }

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }

    private static void updateVisits(Connection con, int userID, int getUserID) {
        CallableStatement st = null;

        try {
            st = con.prepareCall("{call updateVisits(?, ?)}");

            st.setInt(1, userID);
            st.setInt(2, getUserID);

            st.execute();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        finally {
            try {
                if(st!=null) st.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }
    }

    public static int getUserRaiting(Connection con, int userID) {
        ResultSet rsQuestions;
        ResultSet rsComments;
        PreparedStatement stQuestions;
        PreparedStatement stComments;

        int questionsRaiting = 0;
        int commentsRaiting = 0;

        try {
            stQuestions = con.prepareStatement("select dbo.getUserQuestionsRaiting(?)");
            stQuestions.setInt(1, userID);
            rsQuestions = stQuestions.executeQuery();

            if (rsQuestions.next())
                questionsRaiting = rsQuestions.getInt(1);

            stComments = con.prepareStatement("select dbo.getUserCommentsRaiting(?)");
            stComments.setInt(1, userID);
            rsComments = stComments.executeQuery();

            if (rsComments.next())
                commentsRaiting = rsComments.getInt(1);
        } catch (SQLException e) {
            e.printStackTrace();
        }

        int raiting = questionsRaiting + commentsRaiting;

        return raiting;
    }

    public static ResultSet getPosts(Connection con, int userID, int getUserID, int amount) {
        ResultSet rs = null;
        PreparedStatement st = null; //Нормально ли, что я его не закрываю?

        try {
            st = con.prepareStatement("select * from dbo.getPosts(?, ?, ?)");

            st.setInt(1, userID);
            st.setInt(2, getUserID);
            st.setInt(3, amount);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }
}
