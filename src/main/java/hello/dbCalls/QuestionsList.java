package hello.dbCalls;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class QuestionsList {
    /*public static ResultSet getMyQuestions(Connection con, int userID, int amount) {
        ResultSet rs = null;
        PreparedStatement st = null;

        try {
            st = con.prepareStatement("select * from dbo.getMyQuestions(?, ?)");

            st.setInt(1, userID);
            st.setInt(2, amount);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }

    public static ResultSet getFavorites(Connection con, int userID, int amount) {
        ResultSet rs = null;
        PreparedStatement st = null;

        try {
            st = con.prepareStatement("select * from dbo.getFavorites(?, ?)");

            st.setInt(1, userID);
            st.setInt(2, amount);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }

    public static ResultSet getHistory(Connection con, int userID, int amount) {
        ResultSet rs = null;
        PreparedStatement st = null;

        try {
            st = con.prepareStatement("select * from dbo.getHistory(?, ?)");

            st.setInt(1, userID);
            st.setInt(2, amount);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }*/

    public static ResultSet getUserAskedQuestions(Connection con, int userID, int getUserID, int amount, boolean showAnonimous) {
        ResultSet rs = null;
        PreparedStatement st = null;

        try {
            st = con.prepareStatement("select * from dbo.getUserAskedQuestions(?, ?, ?, ?)", ResultSet.TYPE_SCROLL_INSENSITIVE,
                    ResultSet.CONCUR_READ_ONLY);

            st.setInt(1, userID);
            st.setInt(2, getUserID);
            st.setInt(3, amount);
            st.setInt(4, showAnonimous ? 1 : 0);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }

    public static ResultSet getUserAnsweredQuestions(Connection con, int userID, int getUserID, int amount, boolean showAnonimous) {
        ResultSet rs = null;
        PreparedStatement st = null;

        try {
            st = con.prepareStatement("select * from dbo.getUserAnsweredQuestions(?, ?, ?, ?)", ResultSet.TYPE_SCROLL_INSENSITIVE,
                    ResultSet.CONCUR_READ_ONLY);

            st.setInt(1, userID);
            st.setInt(2, getUserID);
            st.setInt(3, amount);
            st.setInt(4, showAnonimous ? 1 : 0);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }

    public static ResultSet getUserFavoriteQuestions(Connection con, int userID, int getUserID, int amount, boolean showAnonimous) {
        ResultSet rs = null;
        PreparedStatement st = null;

        try {
            st = con.prepareStatement("select * from dbo.getUserFavoriteQuestions(?, ?, ?, ?)", ResultSet.TYPE_SCROLL_INSENSITIVE,
                    ResultSet.CONCUR_READ_ONLY);

            st.setInt(1, userID);
            st.setInt(2, getUserID);
            st.setInt(3, amount);
            st.setInt(4, showAnonimous ? 1 : 0);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }
}
