package hello.dbCalls;

import java.sql.*;
import java.util.Calendar;

public class Comments {
    public static void addComment(Connection con, int id, int questionId, String comment, int previousCommentId) {
        CallableStatement st = null;

        try {
            st = con.prepareCall("{call addComment(?, ?, ?, ?, ?)}");

            st.setInt("userID", id);
            st.setInt("questionID", questionId);
            st.setString("comment", comment);

            java.sql.Date commented = new java.sql.Date(Calendar.getInstance().getTime().getTime()); //Доработать, чтобы время было не нулевое всегда
            st.setDate("commented", commented);

            if (previousCommentId > 0)
                st.setInt("previousCommentId", previousCommentId);
            else
                st.setInt("previousCommentId", previousCommentId);

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

    public static void addCommentFeedback(Connection con, int id, int commentId, int feedback) {
        CallableStatement st = null;

        try {
            st = con.prepareCall("{call addCommentFeedback(?, ?, ?)}");

            st.setInt("userID", id);
            st.setInt("commentID", commentId);
            st.setInt("feedback", feedback);

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

    public static ResultSet getComments(Connection con, int id, int questionId, int amount) {
        PreparedStatement st = null;
        ResultSet rs = null;

        try {
            st = con.prepareStatement("select * from dbo.getComments(?, ?, ?)");

            st.setInt(1, id);
            st.setInt(2, questionId);
            st.setInt(3, amount);

            rs = st.executeQuery();

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return rs;
    }
}
